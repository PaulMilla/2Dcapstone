using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewindManager : MonoBehaviour {
	private Stack<Position> recordedEvents;
	public bool isRewinding;
	EnemyGuard guard;

	// Use this for initialization
	void Start () {
		guard = GetComponent<EnemyGuard>();
		recordedEvents = new Stack<Position>();
		isRewinding = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Rewind")) {
			isRewinding = true;
			guard.preRewind();
		}
		else if(Input.GetButtonUp("Rewind")) {
			isRewinding = false;
			guard.postRewind();
		}
	}

	// Save movements via FixedUpdate
	void FixedUpdate() {
		if(isRewinding) {
			Rewind();
		} else {
        	recordedEvents.Push(new Position(this.transform.position, this.transform.rotation));
		}
	}

	void Rewind() {
		if(recordedEvents.Count == 0 || recordedEvents.Peek() == null)
			return;

		Position pastState = recordedEvents.Pop();
		Vector3 past = pastState.position;

		this.transform.position =  past;
		this.transform.rotation = pastState.rotation;

	}

	private class Position {
		public Vector3 position;
		public Quaternion rotation;

		public Position(Vector3 pos, Quaternion rot) {
			position = pos;
			rotation = rot;
		}
	}
}
