using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
	protected float movementSpeed;
	protected Vector3 positionToMoveTo;
	protected bool interacting;
	public bool movementEnabled;

	// Use this for initialization
	protected virtual void Start () {
		movementSpeed = 5.0f;
		movementEnabled = false;
		interacting = false;
	}

	protected void Move() {
		//TODO: Implement interacting
		if ((positionToMoveTo - this.transform.position).magnitude > 0.1f) {
			this.transform.rigidbody.velocity = Vector3.zero;
			this.transform.rotation = Quaternion.LookRotation(positionToMoveTo);
			this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, positionToMoveTo, movementSpeed * Time.fixedDeltaTime));
		}
	}

	public void MoveTo(Vector3 pos) {
		positionToMoveTo = pos;
		positionToMoveTo.y = this.transform.position.y;
	}
}
