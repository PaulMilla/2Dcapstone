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

	public Transform[] patrolWaypoints;

	// An int to keep track of which direction we are along the patrol route
	// If 1, we are incrementing our route
	private int patrolDirection = 1;
	private bool offPatrolRoute = false;


	Vector3 initialPosition;
	Quaternion initialRotation;

	void Start() {
		initialRotation = this.transform.rotation;
		initialPosition = this.transform.position;
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
		if (offPatrolRoute) {
		
		}

		// if we aren't at our station --- 0.1f because there is a cap on how far we move each frame,
		// there may be a case where we cant move any further because the cap is greater than the amount we need to move
		if ((this.transform.position - initialPosition).magnitude > 0.1f) {
			// Setting in motion
			this.transform.rigidbody.velocity = Vector3.zero;
			this.transform.LookAt(initialPosition);
			this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, initialPosition, PursuitSpeed * Time.fixedDeltaTime));
		}
		// We know we have arrived at our station
		else {
			this.transform.rotation = initialRotation;
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
		Debug.Log ("Checking LOS to " + t.name);
		Vector3 direction = t.position - this.transform.position;
		// Raycast to make sure we have straight line of sight
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, direction.normalized, out hit, 1000)) {
			Debug.Log ("Hit Something " + hit.collider.gameObject.name);
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
