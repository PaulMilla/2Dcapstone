using UnityEngine;
using System.Collections;

// This is not really how you are supposed to do AI
// We should actually be using states more efficiently, and making decisions based on the state we are in
// But since the AI is simple, we might not need to do that

public class EnemyGuard : MonoBehaviour {

	[SerializeField]
	EnemyVision vision;

	public float PursuitSpeed;
	public float PatrolSpeed;

	public Transform path;
	Transform[] patrolWaypoints;
	public bool standingGuard = false;

	// An int to keep track of which direction we are along the patrol route
	// If 1, we are incrementing our route
	private int patrolDirection = 1;
	private bool offPatrolRoute = false;
	private int nextWaypointIndex = 0; 

	Vector3 initialPosition;
	Quaternion initialRotation;

	PlayerModel playerModel;

	void Start() {
		vision = GetComponentInChildren<EnemyVision> ();
		patrolWaypoints = path.GetComponentsInChildren<Transform> ();

		playerModel = GetComponent<PlayerModel> ();
		initialRotation = this.transform.rotation;
		initialPosition = this.transform.position;

		// Fix the y axis of the waypoint to our y position
		float fixedY = this.transform.position.y;
		foreach (Transform waypoint in patrolWaypoints) {
			Vector3 fixedPos = waypoint.position;
			fixedPos.y = fixedY;
			waypoint.position = fixedPos;
		}
	}

	void FixedUpdate () {
		// STATE: There is a target to pursue
		if (vision.HasTarget()) {
			Chase ();
		}

		// NO TARGET
		else {
			Patrol();
		}
	}

	void Chase() {
		offPatrolRoute = true;
		Vector3 targetPos = vision.GetTarget().position;
		targetPos.y = this.transform.position.y;  // So we never move in the y direction
		this.transform.rigidbody.velocity = Vector3.zero;
		this.transform.LookAt(targetPos);
		this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, targetPos, PursuitSpeed * Time.fixedDeltaTime));
	}

	void Patrol() {
		// If the guard is standing guard, just go to the initial position
		if (standingGuard) {
			offPatrolRoute = false;
			// If the guard isn't at his station, go to it
			if ((this.transform.position - initialPosition).magnitude > 0.1f) {
				// Setting in motion
				this.transform.rigidbody.velocity = Vector3.zero;
				this.transform.LookAt(initialPosition);
				this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, initialPosition, PursuitSpeed * Time.fixedDeltaTime));
				return;
			}
			// We are already at our position, so face the right way
			else {
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
					if ((patrolWaypoints[i].position - this.transform.position).magnitude < shortestDistance) {
						nextWaypointIndex = i;
						shortestDistance = (patrolWaypoints[i].position - this.transform.position).magnitude;
					}
				}
			}
			// We've made it to our waypoint, so choose another one
			if ((this.transform.position - patrolWaypoints[nextWaypointIndex].position).magnitude <= 0.1f) {
				if (nextWaypointIndex >= patrolWaypoints.Length - 1) {
					patrolDirection = -1;
				}
				else if (nextWaypointIndex == 0) {
					patrolDirection = 1;
				}
				nextWaypointIndex = nextWaypointIndex + patrolDirection;
			}
			Vector3 positionToGoTo = patrolWaypoints[nextWaypointIndex].position;
			this.transform.rigidbody.velocity = Vector3.zero;
			this.transform.LookAt(positionToGoTo);
			this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, positionToGoTo, PatrolSpeed * Time.fixedDeltaTime));
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag.Equals("Player") ||
		    collision.collider.tag.Equals("Hologram")) {
			PlayerModel playerModel = collision.gameObject.GetComponent<PlayerModel>();
			playerModel.GetKilled();
		}
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
}
