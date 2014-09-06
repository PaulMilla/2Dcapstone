using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recording2
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
			return recordedEvents[iteration].GetKeyDown(key);
		}
	}
}

