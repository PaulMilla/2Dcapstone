using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneInput : MonoBehaviour {
	public Stack<Event> recordedInputs {private get; set;}
	private CloneMovement cloneMovement;

	void Start() {
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
		if(Input.GetButtonDown("Rewind")) {
			cloneMovement.Rewind = true;
		}

		if(Input.GetButtonDown("Rewind")) {
			cloneMovement.Rewind = false;
		}
	}

    void FixedUpdate()
    {
		if(recordedInputs.Count == 0 ||
		   recordedInputs.Peek() == null)
			return;
		Event recordedEvent = recordedInputs.Pop();
		cloneMovement.MoveTo(recordedEvent.targetPosition, recordedEvent.interactable);
    }
}