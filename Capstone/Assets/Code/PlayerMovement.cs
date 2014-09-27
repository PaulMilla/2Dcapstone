using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : CharacterMovement {
	private Stack<PlayerState> pastStates;
	private bool rewind;
	public bool Rewind {
		get { return rewind; }
		set {
			rewind = value;
			if (value == false) {
				MoveTo(this.transform.position);
			}
		}
	}

	override protected void Start() {
		base.Start();
		rewind = false;
		pastStates = new Stack<PlayerState>();
	}
	
	void FixedUpdate() {
		if (rewind) {
			DoRewind();
		} else if (movementEnabled) {
			Move();
			pastStates.Push(new PlayerState(this.transform.position, interacting));
		}
	}

	private void DoRewind() {
		if (pastStates.Count != 0) {
			PlayerState previous = pastStates.Pop();
			this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, previous.position, movementSpeed));
		}
	}

	private class PlayerState {
		public Vector3 position {get; set;}
		public bool interacting {get; set;}
		public Quaternion rotation {get; set;} //TODO: Implement rotation
		public PlayerState (Vector3 pos, bool interact) {
			position = pos;
			interacting = interact;
		}
	}
}