using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneInput : CharacterInput
{
	public Stack<Event> recordedInputs {private get; set;}
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
		if(Input.GetKeyDown(rewindKey)) {
			cloneMovement.Rewind = true;
			return;
		}
		if(Input.GetKeyUp(rewindKey)) {
			cloneMovement.Rewind = false;
			return;
		}
		
		if(recordedInputs.Count == 0 ||
		   recordedInputs.Peek() == null)
			return;

		Event recordedEvent = recordedInputs.Pop();
        //Debug.Log("POPING: "+recordedEvent.interactable);
		cloneMovement.MoveTo(recordedEvent.targetPosition, recordedEvent.interactable);
	}
}