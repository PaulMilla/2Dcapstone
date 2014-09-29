using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : CharacterMovement {
	public Stack<Event> cloneEvents {get; private set;}
	private Stack<Event> recordedEvents;
	private bool rewind;
	public bool Rewind {
		set {
			rewind = value;
			if (value) {
				cloneEvents = new Stack<Event>();
			}
			else {
				MoveTo(this.transform.position);
			}
		}
	}

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
			recordedEvents.Push(new Event(this.transform.position, interacting, target));
		}
	}

	private void DoRewind() {
		if (recordedEvents.Count != 0) {
			Event previous = recordedEvents.Pop();
			cloneEvents.Push(previous);

			Vector3 current = this.transform.position;
			Vector3 target = previous.position;
			Vector3 step = Vector3.MoveTowards(current, target, movementSpeed);
			this.transform.rigidbody.MovePosition(step);
		}
	}
}