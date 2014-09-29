using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneInput : CharacterInput
{
	public Stack<Event> recordedEvents {private get; set;}
	private CloneMovement cloneMovement;

	override protected void Start() {
		base.Start();
	}

	void Awake() {
		cloneMovement = GetComponent<CloneMovement>();
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