using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewindManager : MonoBehaviour {
	private Stack<Position> recordedEvents;
	private bool isRewinding;

	// Use this for initialization
	void Start () {
		recordedEvents = new Stack<Position>();
		isRewinding = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Rewind")) {
			GetComponent<EnemyGuard>().movementEnabled = false;
			isRewinding = true;
		} else if(Input.GetButtonUp("Rewind")) {
			GetComponent<EnemyGuard>().movementEnabled = true;
			GetComponent<EnemyGuard>().offPatrolRoute = true;
			isRewinding = false;
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
		Vector3 current = this.transform.position;
		Vector3 past = pastState.position;
		Vector3 step = Vector3.MoveTowards(current, past, GetComponent<EnemyGuard>().PursuitSpeed * Time.fixedDeltaTime);

		this.transform.rigidbody.MovePosition(step);
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
