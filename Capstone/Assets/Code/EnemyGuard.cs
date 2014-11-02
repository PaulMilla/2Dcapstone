using UnityEngine;
using System.Collections;

// This is not really how you are supposed to do AI
// We should actually be using states more efficiently, and making decisions based on the state we are in
// But since the AI is simple, we might not need to do that

public class EnemyGuard : Activatable {

	EnemyVision vision;
	Vector3 lastSeenPosition;

	public float PursuitSpeed;
	public float PatrolSpeed;

	NavMeshAgent agent;

	public Transform path;
	Transform[] patrolWaypoints; 
	public bool standingGuard = false;
	bool chasing = false;

	// An int to keep track of which direction we are along the patrol route
	// If 1, we are incrementing our route, we are going backwards
	public bool offPatrolRoute = false;
	private int nextWaypointIndex = 0; 

	Vector3 initialPosition;
	Quaternion initialRotation;

	RewindManager rewindingManager;

	public float pauseAfterKillTime = 3.0f;
	public bool movementEnabled {get; set;}
	bool pausingAfterKill = false;
	private float pauseAfterKillTimer = 0.0f;

	TextMesh emoticon;
	
	[SerializeField]
	private float investigateTime = 3.0f;
	private float investigateTimer = 0.0f;

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

	private Animator animator;

	protected override void Start() {
		base.Start();

		soundBank = this.transform.FindChild ("SoundBank");
		soundEffectAlert = soundBank.FindChild ("Alert").GetComponent<AudioSource> ();
		soundEffectMotor = soundBank.FindChild ("Motor").GetComponent<AudioSource> ();
		soundEffectMotorStart = soundBank.FindChild ("MotorStart").GetComponent<AudioSource> ();
		soundDialoguesSeesPlayer = soundBank.FindChild ("SeesPlayer").GetComponents<AudioSource> ();
		soundDialoguesBackToPatrol = soundBank.FindChild ("ReturnToPatrol").GetComponents<AudioSource> ();
		soundDialoguesInvestigate = soundBank.FindChild ("Investigating").GetComponents<AudioSource> ();

		soundEffectMotor.Play ();

		animator = GetComponent<Animator>();
		emoticon = GetComponentInChildren<TextMesh> ();
		agent = GetComponent<NavMeshAgent> ();
		movementEnabled = true;
		vision = GetComponentInChildren<EnemyVision> ();
		rewindingManager = GetComponent<RewindManager> ();

		//characterMovement = GetComponent<characterMovement> ();
		initialRotation = this.transform.rotation;
		initialPosition = this.transform.position;

		// Fix the y axis of the waypoint to our y position

		if (!standingGuard) {
			patrolWaypoints = path.GetComponentsInChildren<Transform> ();
			foreach (Transform waypoint in patrolWaypoints) {
				Vector3 fixedPos = waypoint.position;
				waypoint.position = fixedPos;
			}
		}
	}

	public Transform[] GetWaypoints() {
		return patrolWaypoints;
	}

	public void SetNextWaypointIndex(int i) {
		nextWaypointIndex = i;
	}

	public int GetNextWaypointIndex() {
		return nextWaypointIndex;
	}

	public void ResetTarget() {
		vision.ResetTarget ();
		pausingAfterKill = false;
		chasing = false;
	}

	void FixedUpdate () {
		if (rewindingManager.isRewinding) {
			agent.Stop();
			vision.ResetTarget();
			return;
		} 

		if (!Activated || !movementEnabled) {
			return;
		}

		// WE HAVE A CLEAR SIGHT TO TARGET SO PURSUE
		if (vision.HasTarget ()) {
			Chase ();
		} else if (chasing) {
			// NO TARGET BUT WE ARE IN PURSUIT 
			Investigate();
		}
		// NO TARGET AND NOT IN PURSUIT, RETURN TO PATROL
		else {
			Patrol();
		}

		UpdateMotorSound ();
	}

	void UpdateMotorSound() {
		// Volume and Pitch double if guard is in pursuit
		float maxValue = this.agent.speed == PatrolSpeed ? 1.0f : 2.0f;
		this.soundEffectMotor.volume = map(agent.velocity.magnitude, 0.0f, this.agent.speed, 0.0f, maxValue);
		this.soundEffectMotor.pitch = map(agent.velocity.magnitude, 0.0f, this.agent.speed, 0.0f, maxValue);
	}

	void Investigate() {
		if (hasArrivedAt(lastSeenPosition)) {
			// We arrived at the last spot we saw the player

			// This is the first iteration that we are in investigate mode
			if (investigateTimer == 0.0f) {
				playSoundInvestigating();
			}
			this.animator.SetInteger(ANIMATION_NUMBER_STRING, (int)AnimationNumber.Confused);
			if (investigateTimer >= investigateTime) {
				chasing = false;
				investigateTimer = 0.0f;
				playSoundBackToPatrol();
			} else {
				emoticon.text = "?";
				investigateTimer += Time.deltaTime;
			}
		} else {
			// We haven't reached the spot where we last saw the player
			this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Chasing);
			emoticon.text = "Investigating";
			investigateTimer = 0.0f;
			SetDestination(lastSeenPosition, PursuitSpeed);
		}
	}

	void Chase() {
		emoticon.text = "Chase!!!!";
		this.animator.SetInteger (ANIMATION_NUMBER_STRING, (int)AnimationNumber.Chasing); 
		chasing = true;
		offPatrolRoute = true;
		lastSeenPosition = vision.GetTarget ().position;
		SetDestination (lastSeenPosition, PursuitSpeed);
	}

	void Patrol() {
		// If the guard is standing guard, just go to the initial position
		if (!pausingAfterKill) {
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
					this.agent.Stop ();
					this.transform.rotation = initialRotation;
					return;
				}	
			}
			// The guard has a path, so follow it
			else {
				// Guard is off route from chasing the player and needs to pick a point to return to

				if (offPatrolRoute) {
					offPatrolRoute = false;
					// Find the nearest position in the Patrol Route and go there
					float shortestDistance = Mathf.Infinity;
					for (int i = 0; i < patrolWaypoints.Length; i++) {
						if ((patrolWaypoints [i].position - this.transform.position).magnitude < shortestDistance) {
							nextWaypointIndex = i;
							shortestDistance = (patrolWaypoints [i].position - this.transform.position).magnitude;
						}
					}
				}
				// We've made it to our waypoint, so choose another one
				if (this.hasArrivedAt (patrolWaypoints[nextWaypointIndex].position)) {
					soundEffectMotorStart.Play();
					if (nextWaypointIndex >= patrolWaypoints.Length - 1) {
						nextWaypointIndex = 0;
					} else {
						nextWaypointIndex++;
					}
				}
				Vector3 positionToGoTo = patrolWaypoints [nextWaypointIndex].position;
				this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Patrolling);
				SetDestination(positionToGoTo, PatrolSpeed);
			}
		}
		// PAUSING AFTER KILLING THE ENEMY
		else {
			emoticon.text = "Hahaha :)";
			this.animator.SetInteger(ANIMATION_NUMBER_STRING,(int) AnimationNumber.Confused);
			if (pauseAfterKillTimer >= pauseAfterKillTime) {
				pausingAfterKill = false;
				pauseAfterKillTimer = 0.0f;
			} else {
				pauseAfterKillTimer += Time.deltaTime;  
			}
		}
	}

	
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
		if ((pos - this.transform.position).magnitude == 0) {
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
			pausingAfterKill = true;
			chasing = false;
			pauseAfterKillTimer = 0.0f;
		}
	}

	public void SetTarget(Transform t) {
		vision.SetTarget (t);
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
}
