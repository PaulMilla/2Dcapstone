using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : CharacterMovement {
	public Stack<Event> cloneEvents {get; private set;}
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
	}

	void FixedUpdate() {
		if(rewind) {
			cloneEvents.Push(DoRewind());
		} else {
			Move();
		}
	}
}