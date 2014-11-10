using UnityEngine;
using System.Collections;

// This is not really how you are supposed to do AI
// We should actually be using states more efficiently, and making decisions based on the state we are in
// But since the AI is simple, we might not need to do that

public class EnemyGuard : Activatable {
	// An int to keep track of which direction we are along the patrol route
	// If 1, we are incrementing our route, we are going backwards
	public bool offPatrolRoute = false;
	public float PursuitSpeed;
	public float PatrolSpeed;
	public Transform path;
	public bool standingGuard = false;
	public bool movementEnabled {get; set;}

	[SerializeField]
	private const float confusedTime = 3.0f;
	private float confusedTimer = 0.0f;
	public const float pauseAfterKillTime = 3.0f;
	float pauseAfterKillTimer;

	Transform chaseTarget;
	Transform[] patrolWaypoints; 
	int targetWaypoint; 

	NavMeshAgent agent;
	EnemyVision vision;

	Vector3 lastSeenPosition;
	Quaternion initialRotation;

	TextMesh emoticon;

	enum State {
		Idle,
		StandGuard,
		Patrolling,
		Chasing,
		Confused,
		Satisfied,
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
				this.agent.speed = PatrolSpeed;
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Patrolling);
				break;
			case State.Chasing:
				emoticon.text = "Chase!!!!";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING, (int)AnimationNumber.Chasing); 
				offPatrolRoute = true;
				break;
			case State.Confused:
				emoticon.text = "?";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING, (int)AnimationNumber.Confused);
				confusedTimer = confusedTime;
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
			case State.Investigating:
				emoticon.text = "Investigating";
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Chasing);
				playSoundInvestigating();
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

	protected override void Start() {
		base.Start();

		// Sound stuff
		soundBank = this.transform.FindChild ("SoundBank");
		soundEffectAlert = soundBank.FindChild ("Alert").GetComponent<AudioSource> ();
		soundEffectMotor = soundBank.FindChild ("Motor").GetComponent<AudioSource> ();
		soundEffectMotorStart = soundBank.FindChild ("MotorStart").GetComponent<AudioSource> ();
		soundDialoguesSeesPlayer = soundBank.FindChild ("SeesPlayer").GetComponents<AudioSource> ();
		soundDialoguesBackToPatrol = soundBank.FindChild ("ReturnToPatrol").GetComponents<AudioSource> ();
		soundDialoguesInvestigate = soundBank.FindChild ("Investigating").GetComponents<AudioSource> ();
		soundEffectMotor.Play ();

		// Initialize Components
		animator = GetComponent<Animator>();
		emoticon = GetComponentInChildren<TextMesh>();
		agent = GetComponent<NavMeshAgent>();
		vision = GetComponentInChildren<EnemyVision>();

		// Initialize default variables
		movementEnabled = true;
		targetWaypoint = 0;
		pauseAfterKillTimer = 0.0f;
		offPatrolRoute = false;
		initialRotation = this.transform.rotation;
		if(patrolWaypoints == null) {
			patrolWaypoints = new Transform[]{this.transform};
		}

		// Fix the y axis of the waypoint to our y position
		if (!standingGuard) {
			patrolWaypoints = path.GetComponentsInChildren<Transform>();
			foreach (Transform waypoint in patrolWaypoints) {
				Vector3 fixedPos = waypoint.position;
				fixedPos.y = this.transform.position.y;
				waypoint.position = fixedPos;
			}
		}

		myState = State.Patrolling;
	}

	void FixedUpdate () {
		// Implies that we are being moved by some other force (RewindManager, etc)
		if (!Activated || !movementEnabled) {
			if (!standingGuard && hasArrivedAt(GetPreviousWaypoint())) {
				if(--targetWaypoint < 0)
					targetWaypoint = patrolWaypoints.Length-1;
			}
			agent.Stop();
			return;
		}

		/*
		if (vision.HasTarget()) { 	// WE HAVE A CLEAR SIGHT TO TARGET SO PURSUE
			Chase();
		} else if (chasing) { 		// NO TARGET BUT WE ARE IN PURSUIT
			Investigate();
		} else { 					// NO TARGET AND NOT IN PURSUIT, RETURN TO PATROL
			Patrol();
		}
        */

		// NEW STUFF
		switch(myState) {
		case State.Idle:
			break;
		case State.Patrolling:	Patrol(); break;
		case State.Chasing:		Chase(); break;
		case State.Confused:	Confused(); break;
		case State.Satisfied:	Satisfied(); break;
		case State.StandGuard:	StandGuard(); break;
		case State.Investigating: Investigate(); break;
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
		offPatrolRoute = true; //TODO: Check if we really need this check

		// The hologram we were chasing suddenly dissappeared
		if(chaseTarget == null)
			myState = State.Confused;
		else if (HasLineOfSightTo(chaseTarget))
			lastSeenPosition = chaseTarget.position;
		else
			myState = State.Investigating;

		SetDestination(lastSeenPosition, PursuitSpeed);
	}

	void Patrol() {
		// Guard is off route from chasing the player and needs to pick a point to return to
		if (offPatrolRoute) {
			offPatrolRoute = false;
			soundEffectMotorStart.Play();
			targetWaypoint = ClosestWaypoint();
		}

		// We've made it to our waypoint, so choose another one
		if (this.hasArrivedAt (patrolWaypoints[targetWaypoint].position)) {
			if (standingGuard) {
				myState = State.StandGuard;
				return;
			}
			soundEffectMotorStart.Play();
			targetWaypoint = (targetWaypoint + 1) % patrolWaypoints.Length;
		}

		// Start moving to our new location
		Vector3 positionToGoTo = patrolWaypoints[targetWaypoint].position;
		SetDestination(positionToGoTo, PatrolSpeed);
	}

	void Satisfied() {
		pauseAfterKillTimer -= Time.deltaTime;
		if (pauseAfterKillTimer <= 0.0f) {
			myState = State.Patrolling;
		}
	}

	void Confused() {
		confusedTimer -= Time.deltaTime;
		if(confusedTimer <= 0.0f) {
			myState = State.Patrolling;
			playSoundBackToPatrol();
		}
	}

	void StandGuard() {
		this.transform.rotation = initialRotation;
		this.agent.Stop();
	}

	void Investigate() {
		if(hasArrivedAt(lastSeenPosition)) {
			myState = State.Confused;
		} else {
			SetDestination(lastSeenPosition, PursuitSpeed);
		}
	}

	int ClosestWaypoint() {
		// Find the nearest position in the Patrol Route and go there
		int closest = 0;
		float shortestDistance = Mathf.Infinity;
		for (int i = 0; i < patrolWaypoints.Length; i++) {
			if ((patrolWaypoints [i].position - this.transform.position).magnitude < shortestDistance) {
				closest = i;
				shortestDistance = (patrolWaypoints [i].position - this.transform.position).magnitude;
			}
		}
		return closest;
	}

/*
	void Investigating() {
		// We arrived at the last spot we saw the player
		if (hasArrivedAt(lastSeenPosition)) {
			// This is the first iteration that we are in investigate mode
			if (confusedTimer == 0.0f) {
				playSoundInvestigating();
			}

			this.animator.SetInteger(ANIMATION_NUMBER_STRING, (int)AnimationNumber.Confused);
			if (confusedTimer >= confusedTime) {
				confusedTimer = 0.0f;
				playSoundBackToPatrol();
			} else {
				emoticon.text = "?";
				confusedTimer += Time.deltaTime;
			}
		} else {
			// We haven't reached the spot where we last saw the player
			this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Chasing);
			emoticon.text = "Investigating";
			confusedTimer = 0.0f;
			SetDestination(lastSeenPosition, PursuitSpeed);
		}
	}

	void Patrolling() {
		// PAUSE AFTER KILLING THE ENEMY
		if (isPausingAfterKill) {
			emoticon.text = "Hahaha :)";
			this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Confused);
			if (pauseAfterKillTimer >= pauseAfterKillTime) {
				isPausingAfterKill = false;
				pauseAfterKillTimer = 0.0f;
			} else {
				pauseAfterKillTimer += Time.deltaTime;  
			}

			return;
		}

		// Else if the guard is standing guard, just go to the initial position
		emoticon.text = "Patrolling";
		this.agent.speed = PatrolSpeed;
		if (standingGuard) {
			offPatrolRoute = false;
			// If the guard isn't at his station, go to it
			if (!hasArrivedAt(initialPosition)) {
				// Setting in motion
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Patrolling);
				SetDestination(initialPosition, PatrolSpeed);
				return;
			}
			// We are already at our position, so face the right way
			else {
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Idle);
				this.agent.Stop();
				this.transform.rotation = initialRotation;
				return;
			}	
		}

		// The guard has a path, so follow it
		else if (patrolWaypoints != null) {
			// Guard is off route from chasing the player and needs to pick a point to return to
			if (offPatrolRoute) {
				offPatrolRoute = false;
				// Find the nearest position in the Patrol Route and go there
				float shortestDistance = Mathf.Infinity;
				for (int i = 0; i < patrolWaypoints.Length; i++) {
					if ((patrolWaypoints [i].position - this.transform.position).magnitude < shortestDistance) {
						targetWaypoint = i;
						shortestDistance = (patrolWaypoints [i].position - this.transform.position).magnitude;
					}
				}
			}
			// We've made it to our waypoint, so choose another one
			if (this.hasArrivedAt (patrolWaypoints[targetWaypoint].position)) {
				soundEffectMotorStart.Play();
				if (targetWaypoint >= patrolWaypoints.Length - 1) {
					targetWaypoint = 0;
				} else {
					targetWaypoint++;
				}
			}
			Vector3 positionToGoTo = patrolWaypoints [targetWaypoint].position;
			this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Patrolling);
			SetDestination(positionToGoTo, PatrolSpeed);
		}
	}
*/
	public void SetDestination(Vector3 pos, float speed) {
		Vector3 targetPos = pos;
		targetPos.y = this.transform.position.y;  // So we never move in the y direction
		this.transform.rigidbody.velocity = Vector3.zero;
		this.transform.LookAt(targetPos);
		this.agent.speed = speed;
		this.agent.SetDestination (targetPos);
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

	public void SetTarget(Transform t) {
		vision.SetTarget (t);
	}

	public void ChaseTarget(Transform t) {
		chaseTarget = t;
		myState = State.Chasing;
	}

	public bool HasLineOfSightTo(Transform t) {
		Vector3 direction = t.position - this.transform.position;
		// Raycast to make sure we have straight line of sight
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, direction.normalized, out hit, 1000)) {
			if (hit.transform.Equals (t)) {
				return true;
			}
			else {
				return false;
			}
		}
		return false;
	}

	public void playSoundAlert() {
		soundEffectAlert.Play ();
	}

	public void playSoundSeesPlayer() {
		int randomIndex = Random.Range (0, soundDialoguesSeesPlayer.Length - 1);
		soundDialoguesSeesPlayer [randomIndex].Play ();
		playSoundAlert ();
	}

	public void playSoundBackToPatrol() {
		int randomIndex = Random.Range (0, soundDialoguesBackToPatrol.Length - 1);
		soundDialoguesBackToPatrol [randomIndex].Play ();
	}

	public void playSoundInvestigating() {
		int randomIndex = Random.Range (0, soundDialoguesInvestigate.Length - 1);
		soundDialoguesInvestigate [randomIndex].Play ();
	}

	float map(float s, float a1, float a2, float b1, float b2) {
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}


	private int GetNextWaypoint() {
		return (targetWaypoint+1) % patrolWaypoints.Length;
	}

	private Vector3 GetPreviousWaypoint() {
		if (targetWaypoint - 1 < 0)
			return patrolWaypoints[patrolWaypoints.Length - 1].position;
		else
			return patrolWaypoints[targetWaypoint - 1].position;
	}

	public void preRewind() {
		movementEnabled = false;
	}

	public void postRewind() {
		movementEnabled = true;
        offPatrolRoute = false;
	}
}
