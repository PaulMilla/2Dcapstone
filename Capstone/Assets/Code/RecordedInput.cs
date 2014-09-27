using UnityEngine;
using System.Collections;

public class RecordedInput : CharacterInput
{
	// Holds a list of recorded actions we can iterate through every frame
	public Recording recording { get; set; }

	// This keeps track of which frame we are on
	// it gets updated every FixedUpdate
	int iteration = 0;

	void Awake() {
		playerMovement = gameObject.GetComponent<PlayerMovement>() as PlayerMovement;
	}
	
	void FixedUpdate() {
		if (GameManager.Instance.inRound) {
			GetInput();
			iteration++;
		}
	}


	// The difference is here really Instead of using Input.GetKey, we can
	// use our recorded input for this frame.
	void GetInput() {
		if (recording.GetMouseButtonDown(iteration, 0)) {
			Ray ray = Camera.main.ScreenPointToRay(recording.GetMousePosition(iteration));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor")))
			{
				playerMovement.MoveTo(hit.point);
			}
		}

		interactionButtonDown = false;
		if (recording.GetKeyDown(iteration, KeyCode.E)) {
			interactionButtonDown = true;
		}	
	}
}