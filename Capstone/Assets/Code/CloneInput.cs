using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneInput : CharacterInput
{
	private Stack<Event> recordedEvents;
	private CloneMovement cloneMovement;

	override protected void Start() {
		base.Start();
	}

	void Awake() {
		cloneMovement = GetComponent<CloneMovement>();
	}

	public void SetEvents(Stack<Event> events) {
		recordedEvents = events;
	}
	
	void Update() {
		if (GameManager.Instance.inRound) {
			ReadInput();
		}
	}

	void ReadInput() {
		if(recordedEvents.Count == 0)
			return;
		Event recordedEvent = recordedEvents.Pop();
		cloneMovement.MoveTo(recordedEvent.target);
	}
}