using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recording
{
	List<RecordedEvent> recordedEvents = new List<RecordedEvent>();


	public void AddEvent (RecordedEvent recordedEvent)
	{
		recordedEvents.Add(recordedEvent);
	}

	public int NumEvents() {
		return recordedEvents.Count;
	}


	// Our own inplementation of Input.GetKey()
	// The iteration parameter is going to be the number of the 
	// FixedUpdate iteration
	public bool GetKey(int iteration, KeyCode key) {
		if (iteration >= recordedEvents.Count)
			return false;
		else {
			return recordedEvents[iteration].GetKey(key);
		}
	}

	// Our implementation of Input.GetKeyDown()
	public bool GetKeyDown(int iteration, KeyCode key) {
		if (iteration >= recordedEvents.Count)
			return false;
		else {
			return recordedEvents[iteration].GetKeyDown(key);
		}
	}
}

