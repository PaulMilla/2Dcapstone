using UnityEngine;
using System.Collections;

// This is not really how you are supposed to do AI
// We should actually be using states more efficiently, and making decisions based on the state we are in
// But since the AI is simple, we might not need to do that

public class EnemyGuard : Activatable {
	public Transform path;
	public float PursuitSpeed = 7.0f;
	public float PatrolSpeed  = 3.0f;
	public float RotationSpeed = 180.0f;
	public bool standingGuard = false;

	public float confusedTime = 3.0f;
	public float pauseAfterKillTime = 3.0f;
	public float hesitateTime = 0.3f;
	private float confusedTimer;
	private float pauseAfterKillTimer;
	private float hesitateTimer;

	bool offPatrolRoute = false;

	int targetWaypoint; 
	Vector3[] patrolWaypoints; 

	NavMeshAgent agent;
	TextMesh emoticon;

	Transform chaseTarget;
	Vector3 lastSeenPosition;
	Quaternion initialRotation;

	enum State {
		Idle,
		StandGuard,
		Patrolling,
		Chasing,
		Confused,
        Hesitating,
		Satisfied,
        Rewinding,
		Investigating
	}
	State _myState;
	State myState {
		get{ return _myState; }
		set{
			_myState = value;
			switch(value) {
			case State.Idle:
				break;
			case State.Patrolling:
				emoticon.text = "Patrolling";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Patrolling);
				this.agent.speed = PatrolSpeed;
                offPatrolRoute = false;
				break;
			case State.Chasing:
				emoticon.text = "Chase!!!!";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING, (int)AnimationNumber.Chasing); 
                this.agent.speed = PursuitSpeed;
				offPatrolRoute = true;
				break;
			case State.Confused:
				emoticon.text = "?";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING, (int)AnimationNumber.Confused);
				confusedTimer = confusedTime;
				break;
            case State.Hesitating:
                emoticon.text = "!";
                //TODO: Animation?
                hesitateTimer = hesitateTime;
                break;
			case State.Satisfied:
				emoticon.text = "Hahaha :)";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Confused);
				pauseAfterKillTimer = pauseAfterKillTime;
				break;
			case State.StandGuard:
				emoticon.text = "Standing Guard!";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Idle);
				break;
            case State.Rewinding:
                break;
			case State.Investigating:
				emoticon.text = "Investigating";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Chasing);
				break;
			}
		}
	}

	private Animator animator;
	private const string ANIMATION_NUMBER_STRING = "AnimationNumber";
	enum AnimationNumber {
		Idle,
		Patrolling,
		Chasing, 
		Confused
	}

	private Transform soundBank;
	private AudioSource soundEffectAlert;
	private AudioSource soundEffectMotor;
	private AudioSource soundEffectMotorStart;
	private AudioSource[] soundDialoguesSeesPlayer;
	private AudioSource[] soundDialoguesBackToPatrol;
	private AudioSource[] soundDialoguesInvestigate;
	private AudioSource[] soundDialoguesCelebrate;

	protected override void Start() {
		base.Start();

		GameState.StartPaused += () => { agent.Stop(); };
		GameState.EndPaused += () => { agent.Resume(); };
		// Sound stuff
		soundBank = this.transform.FindChild ("SoundBank");
		soundEffectAlert = soundBank.FindChild ("Alert").GetComponent<AudioSource> ();
		soundEffectMotor = soundBank.FindChild ("Motor").GetComponent<AudioSource> ();
		soundEffectMotorStart = soundBank.FindChild ("MotorStart").GetComponent<AudioSource> ();
		soundDialoguesSeesPlayer = soundBank.FindChild ("SeesPlayer").GetComponents<AudioSource> ();
		soundDialoguesBackToPatrol = soundBank.FindChild ("ReturnToPatrol").GetComponents<AudioSource> ();
		soundDialoguesInvestigate = soundBank.FindChild ("Investigating").GetComponents<AudioSource> ();
		soundDialoguesCelebrate = soundBank.FindChild ("Celebrating").GetComponents<AudioSource> ();
		soundEffectMotor.Play ();

		// Initialize Components
		animator = GetComponent<Animator>();
		emoticon = GetComponentInChildren<TextMesh>();
		agent = GetComponent<NavMeshAgent>();

		// Initialize default variables
		targetWaypoint = 0;
        confusedTimer = 0.0f;
        hesitateTimer = 0.0f;
		pauseAfterKillTimer = 0.0f;
		offPatrolRoute = false;
		initialRotation = this.transform.rotation;

		if(path == null || standingGuard) {
			patrolWaypoints = new Vector3[]{this.transform.position};
			standingGuard = true;
		} else {
			Transform[] pathPoints = path.GetComponentsInChildren<Transform>();
			patrolWaypoints = new Vector3[pathPoints.Length];
			for (int i = 0; i < pathPoints.Length; ++i) {
				patrolWaypoints[i] = pathPoints[i].position;
				patrolWaypoints[i].y = this.transform.position.y;
			}
		}

		myState = State.Patrolling;
	}

	void FixedUpdate () {
		if (GameState.Paused) {
			return;
		}
        // Should probably be turned into it's own state?
		if (!Activated) {
			return;
		}

		switch(myState) {
		case State.Idle: break;
		case State.Patrolling:	Patrol(); break;
		case State.Chasing:		Chase(); break;
		case State.Confused:	Confused(); break;
        case State.Hesitating:  Hesitate(); break;
		case State.Satisfied:	Satisfied(); break;
		case State.StandGuard:	StandGuard(); break;
        case State.Rewinding:   CheckRewind(); break;
		case State.Investigating: Investigating(); break;
		}

		UpdateMotorSound();
	}


	float maxValPatrol = 0.5f;
	float maxValPursuit = 1.0f;
	void UpdateMotorSound() {
		// Volume and Pitch double if guard is in pursuit
		float maxValue = this.agent.speed == PatrolSpeed ? maxValPatrol : maxValPursuit;
		this.soundEffectMotor.volume = map(agent.velocity.magnitude, 0.0f, this.agent.speed, 0.0f, maxValue);
		this.soundEffectMotor.pitch = map(agent.velocity.magnitude, 0.0f, this.agent.speed, 0.0f, maxValue);
	}

	void Chase() {
        if (chaseTarget == null || !HasLineOfSightTo(chaseTarget))
            myState = State.Investigating;
        else
            lastSeenPosition = chaseTarget.position;

		SetDestination(lastSeenPosition);
	}

	void Patrol() {
		// Guard is off route from chasing the player and needs to pick a point to return to
		if (offPatrolRoute) {
			offPatrolRoute = false;
			soundEffectMotorStart.Play();
			targetWaypoint = ClosestWaypoint();
		}

		// We've made it to our waypoint, so choose another one
		if (this.hasArrivedAt (patrolWaypoints[targetWaypoint])) {
			if (standingGuard) {
				myState = State.StandGuard;
				return;
			}
			soundEffectMotorStart.Play();
			targetWaypoint = (targetWaypoint + 1) % patrolWaypoints.Length;
		}

		/**
		 * Start moving to our target position. Small rotations are taken care of in
		 * SetDestination. For any rotations larget than 10 degrees we take the time
		 * to stop and rotate in place before continue moving.
		 */
		Vector3 positionToGoTo = patrolWaypoints[targetWaypoint];
		//Vector3 direction = (positionToGoTo - transform.position).normalized;
		//Quaternion lookRotation = Quaternion.LookRotation(direction);
		//float angleDifference = Vector3.Angle(transform.forward, direction);
		/**
		 * Snap the last 10 angles to allow for leeway
		 * Multiply by Time.deltaTime to allow us to freeze when setting deltaTime to 0
		 */
		//if (angleDifference > 15.0f) {
			//transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, RotationSpeed*Time.deltaTime);
		//} else {
			SetDestination(positionToGoTo);
		//}
	}

	void Satisfied() {
		if (pauseAfterKillTimer == pauseAfterKillTime) {
			playSoundCelebrate();
		}
		pauseAfterKillTimer -= Time.deltaTime;
		if (pauseAfterKillTimer <= 0.0f) {
			myState = State.Patrolling;
		}
        agent.Stop();
	}

	void Confused() {
		if (confusedTimer == confusedTime) {
			playSoundConfused();
		}
		confusedTimer -= Time.deltaTime;
		if(confusedTimer <= 0.0f) {
			playSoundBackToPatrol();
			myState = State.Patrolling;
		}
        agent.Stop();
	}

    void Hesitate() {
        hesitateTimer -= Time.deltaTime;
		this.transform.LookAt(chaseTarget);
        if (hesitateTimer <= 0.0f) {
            myState = State.Chasing;
        }
        agent.Stop();
    }

	void StandGuard() {
		this.transform.rotation = initialRotation;
		this.agent.Stop();
	}

    void CheckRewind() {
        if (!standingGuard && hasArrivedAt(GetPreviousWaypoint())) {
            targetWaypoint -= 1;
            if(targetWaypoint < 0)
                targetWaypoint = patrolWaypoints.Length-1;
        }
        agent.Stop();
    }

	void Investigating() {
		if(hasArrivedAt(lastSeenPosition)) {
			myState = State.Confused;
		} else {
			SetDestination(lastSeenPosition);
		}
	}

	/**
	 * We want to investigate the spot at the next available time. We don't want to
	 * immediately send the Guard into Investigating mode in case we are still
	 * hesitating/pausing for kill/confused/etc.
	 */
    public void Investigate(Vector3 spot) {
        lastSeenPosition = spot;
		chaseTarget = null;
    }

	int ClosestWaypoint() {
		// Find the nearest position in the Patrol Route
		int closest = 0;
		float shortestDistance = Mathf.Infinity;
		for (int i = 0; i < patrolWaypoints.Length; i++) {
			if ((patrolWaypoints[i] - this.transform.position).magnitude < shortestDistance) {
				closest = i;
				shortestDistance = (patrolWaypoints[i] - this.transform.position).magnitude;
			}
		}
		return closest;
	}

	public void SetDestination(Vector3 pos, float speed) {
		this.agent.speed = speed;
        SetDestination(pos);
	}

    private void SetDestination(Vector3 pos) {
		Vector3 targetPos = pos;
		targetPos.y = this.transform.position.y;  // So we never move in the y direction
		this.transform.rigidbody.velocity = Vector3.zero;
		Vector3 direction = (targetPos - transform.position).normalized;
		Quaternion _lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * this.agent.speed);
		this.agent.SetDestination(targetPos);

    }

	public bool hasArrivedAt(Vector3 pos) {
		pos.y = this.transform.position.y;
		if ((pos - this.transform.position).magnitude <= 0.01f) {
			return true;
		}
		return false;
	}

	void OnCollisionEnter(Collision collision) {
		if (!Activated) {
			return;
		}
		if (collision.collider.tag.Equals("Player") || collision.collider.tag.Equals("Hologram")) {
			CharacterStatus characterStatus = collision.gameObject.GetComponent<CharacterStatus>();
			characterStatus.Hit(2);
			agent.Stop();
			myState = State.Satisfied;
		}
	}

	public void FoundTarget(Transform target) {
		chaseTarget = target;
        lastSeenPosition = target.position;
		switch(myState) {
		case State.Chasing: 		break;
		case State.Investigating: 	myState = State.Chasing; break;
		default: 					myState = State.Hesitating; break;
		}
	}

	public bool InVisionCone(Transform t) {
		Vector3 direction = t.position - this.transform.position;
		float angle = Vector3.Angle(direction, this.transform.forward);

		if (angle < EnemyVision.fieldOfViewAngle+10f) {
			if (this.HasLineOfSightTo(t)) {
				return true;
			}
			else {
				return false;
			}
		} return false;
	}

	public bool HasLineOfSightTo(Transform t) {
		Vector3 direction = t.position - this.transform.position;
		// Raycast to make sure we have straight line of sight
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, direction.normalized, out hit, 1000)) {
			if (hit.transform.Equals (t)) {
				return true;
			} else {
				return false;
			}
		}
		return false;
	}

	public void playSoundAlert() {
		soundEffectAlert.Play ();
	}

	public void playSoundSeesPlayer() {
		int randomIndex = Random.Range (0, soundDialoguesSeesPlayer.Length);
		soundDialoguesSeesPlayer [randomIndex].Play ();
		playSoundAlert ();
	}

	public void playSoundBackToPatrol() {
		int randomIndex = Random.Range (0, soundDialoguesBackToPatrol.Length);
		soundDialoguesBackToPatrol [randomIndex].Play ();
	}

	public void playSoundConfused() {
		int randomIndex = Random.Range (0, soundDialoguesInvestigate.Length);
		soundDialoguesInvestigate [randomIndex].Play ();
	}

	public void playSoundCelebrate() {
		int randomIndex = Random.Range (0, soundDialoguesCelebrate.Length);
		soundDialoguesCelebrate [randomIndex].Play ();
	}

	float map(float s, float a1, float a2, float b1, float b2) {
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}

	private int GetNextWaypoint() {
		return (targetWaypoint+1) % patrolWaypoints.Length;
	}

	private Vector3 GetPreviousWaypoint() {
		if (targetWaypoint - 1 < 0)
			return patrolWaypoints[patrolWaypoints.Length - 1];
		else
			return patrolWaypoints[targetWaypoint - 1];
	}

	public void preRewind() {
        myState = State.Rewinding;
	}

	public void postRewind() {
        myState = State.Patrolling;
	}
}
