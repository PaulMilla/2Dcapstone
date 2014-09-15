using UnityEngine;
using System.Collections;

public class EnemyGuard : MonoBehaviour {

	[SerializeField]
	EnemyVision vision;

	public float movementSpeed;

	void Start () {
		
	}
	
	void FixedUpdate () {
		if (vision.HasTarget()) {
			// Move towards the target
			Vector3 direction = vision.GetTarget().position - this.transform.position;
			direction.z = this.transform.position.z;
			Debug.Log ("Moving to player");
			transform.rigidbody.MovePosition(transform.position + (Vector3)(direction * movementSpeed * Time.fixedDeltaTime));
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
