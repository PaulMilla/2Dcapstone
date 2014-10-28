using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewindManager : MonoBehaviour {
	private Stack<Position> recordedEvents;
	public bool isRewinding;
	EnemyGuard guard;

	int nextWaypointIndex;

	// Use this for initialization
	void Start () {
		guard = GetComponent<EnemyGuard> ();
		recordedEvents = new Stack<Position>();
		isRewinding = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Rewind")) {
			guard.movementEnabled = false;
			isRewinding = true;
			nextWaypointIndex = guard.GetNextWaypointIndex();
		}
		else if(Input.GetButtonUp("Rewind")) {
			guard.movementEnabled = true;
			isRewinding = false;
			guard.ResetTarget();
			guard.SetNextWaypointIndex(nextWaypointIndex);
		}
	}

	int GetNextWaypointIndex() {
		Transform[] waypoints = guard.GetWaypoints();
		for (int i = 0; i < waypoints.Length; i ++) {
			if (guard.hasArrivedAt(waypoints[i].position)) {
				Debug.Log("Waypoint index: " + i);
				return i;
			}
		} return nextWaypointIndex;
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

		if (!guard.standingGuard) {
			nextWaypointIndex = GetNextWaypointIndex();
		}
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
