using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : CharacterMovement {
	public Stack<Event> cloneEvents {get; private set;}
	private Stack<Event> recordedEvents;
	private bool rewind;
	public bool Rewind {
		get { return rewind; }
		set {
			rewind = value;
			if (rewind) {
				cloneEvents = new Stack<Event>();
			} else {
				ClearTarget();
			}
		}
	}

	/* Inherited from CharacterMovement */
	/* overrive protected void Move() */
	/* override public void MoveTo(RaycastHit target) */

	override protected void Start() {
		base.Start();
		rewind = false;
		recordedEvents = new Stack<Event>();
	}

	void FixedUpdate() {
		if(rewind) {
			DoRewind();
		} else {
			Move();
			recordedEvents.Push(new Event(target, this.transform));
		}
	}
	
	private void DoRewind() {
		if (recordedEvents.Count == 0)
			return;
		Event previous = recordedEvents.Pop();
		cloneEvents.Push(previous);

		this.transform.rotation = previous.rotation;
		Vector3 current = this.transform.position;
		Vector3 past = previous.position;
		Vector3 step = Vector3.MoveTowards(current, past, movementSpeed * Time.fixedDeltaTime);
		this.transform.rigidbody.MovePosition(step);
		CheckActivations();
	}
}