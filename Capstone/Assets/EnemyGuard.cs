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

	PlayerModel playerModel;

	// Actually we should have a GuardPath script to attach to this gameobject
	Vector3 initialPosition;
	Quaternion initialRotation;

	void Start() {
		playerModel = GetComponent<PlayerModel> ();
		initialRotation = this.transform.rotation;
		initialPosition = this.transform.position;
	}

	void FixedUpdate () {

		// STATE: There is a target to pursue
		if (vision.HasTarget()) {
			// Move towards the target
			Vector3 targetPos = vision.GetTarget().position;
			targetPos.y = this.transform.position.y;  // So we never move in the y direction

			// Setting in motion
			this.transform.rigidbody.velocity = Vector3.zero;
			this.transform.LookAt(targetPos);
			this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, targetPos, PursuitSpeed * Time.fixedDeltaTime));
		}

		// NO TARGET
		else {
			ReturnToDuty();
		}
	}

	void ReturnToDuty() {
		// if we aren't at our station --- 0.1f because there is a cap on how far we move each frame,
		// there may be a case where we cant move any further because the cap is greater than the amount we need to move
		if ((this.transform.position - initialPosition).magnitude > 0.1f) {
			// Setting in motion
			this.transform.rigidbody.velocity = Vector3.zero;
			this.transform.LookAt(initialPosition);
			this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, initialPosition, PatrolSpeed * Time.fixedDeltaTime));
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
