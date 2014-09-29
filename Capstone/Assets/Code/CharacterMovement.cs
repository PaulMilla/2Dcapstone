using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
	[SerializeField]
	protected float movementSpeed;
	protected Vector3 target;
	protected bool interacting;
	public bool movementEnabled {get;set;}

	// Use this for initialization
	protected virtual void Start () {
		//movementSpeed = 5.0f;
		movementEnabled = false;
		interacting = false;
	}

	protected virtual void Move() {
		//TODO: Implement interacting
		if (movementEnabled) {
			if ((target - this.transform.position).magnitude > 0.1f) {
				this.transform.rigidbody.velocity = Vector3.zero;
				this.transform.rotation = Quaternion.LookRotation(target);
				this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, target, movementSpeed * Time.fixedDeltaTime));
			}
		}
	}

	public void MoveTo(Vector3 pos) {
		if(pos == target)
			return;
		target = pos;
		target.y = this.transform.position.y;
	}
}
