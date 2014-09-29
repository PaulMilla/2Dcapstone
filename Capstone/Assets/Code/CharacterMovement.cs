using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour {
	[SerializeField]
	protected float movementSpeed;
	protected RaycastHit target;
	public float interactionRange;
	public bool movementEnabled {get; set;}
	protected bool rewind;
	public Stack<Event> recordedEvents;

	protected virtual void Start () {
		movementEnabled = false;
		rewind = false;
		recordedEvents = new Stack<Event>();
	}

	protected virtual void Move() {
		Transform current = this.transform;
		if(!movementEnabled || (target.point - current.position).magnitude <= 0.1f)
			return;
		current.rotation = Quaternion.LookRotation(target.point);
		current.rigidbody.velocity = Vector3.zero;
		current.rigidbody.MovePosition(Vector3.MoveTowards(current.position, target.point, movementSpeed * Time.fixedDeltaTime));
		recordedEvents.Push(new Event(target, this.transform));
		CheckActivations();
	}
	
	protected virtual Event DoRewind() {
		if (recordedEvents.Count == 0)
			return null;

		Event previous = recordedEvents.Pop();

		this.transform.rotation = previous.rotation;
		Vector3 current = this.transform.position;
		Vector3 past = previous.position;
		Vector3 step = Vector3.MoveTowards(current, past, movementSpeed * Time.fixedDeltaTime);
		this.transform.rigidbody.MovePosition(step);
		CheckActivations(); //TODO: Move this off the player and to an interactable object
		return previous;
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
		bool inRange = target.collider.Raycast(new Ray(this.transform.position, target.point), out temp, interactionRange);
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
