using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
	[SerializeField]
	protected float movementSpeed;
	protected RaycastHit target;
	public float interactionRage;
	public bool movementEnabled {get; set;}

	protected virtual void Start () {
		movementEnabled = false;
	}

	protected virtual void Move() {
		Transform current = this.transform;
		if(!movementEnabled || (target.point - current.position).magnitude <= 0.1f)
			return;
		current.rotation = Quaternion.LookRotation(target.point);
		current.rigidbody.velocity = Vector3.zero;
		current.rigidbody.MovePosition(Vector3.MoveTowards(current.position, target.point, movementSpeed * Time.fixedDeltaTime));
		CheckActivations();
	}

	public virtual void MoveTo(RaycastHit hit) {
		if(hit.Equals(target))
			return;
		target = hit;
		//target.point.y = this.transform.position.y;
	}
	
	public virtual void CheckActivations() {
		if (target.collider == null || target.transform == null)
			return;

		//TODO: Make sure the you can't flip a switch behind the wall
		RaycastHit temp;
		bool inRange = target.collider.Raycast(new Ray(this.transform.position, target.point), out temp, interactionRage);
		if(target.transform.tag.Equals("Activatable") && inRange) {
			//TODO: Activate the target
			ClearTarget();
		}
	}

	public virtual void ClearTarget() {
		RaycastHit newTarget;
		Physics.Raycast(this.transform.position, Vector3.up, out newTarget, 100f);
		target = newTarget;
		target.point = this.transform.position;
	}
}
