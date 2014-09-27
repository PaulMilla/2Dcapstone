using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : CharacterInput {
	//public RecordedInput recordedInput {get; set;}
	//private Recording recording;
	private List<InputEvent> recording;
	private int current;

	public void OnRoundStart() {
		//recording = new Recording();
		//RecordingManager.Instance.AddRecording(recording);
		recording = new List<InputEvent>();
		current = 0;
	}

	void Update() {
		if(GameManager.Instance.inRound) {
			ReadInput();
		}
	}

	/* Input.Get() functions operate on a single frame basis.
	 * As such, the variant frame rates of FixedUpdate() will sometimes
	 * cause inputs to not be read/missed.
	 */
	void FixedUpdate() {
		//if (GameManager.Instance.inRound) {
		//	ReadInput();
		//}
	}

	void ReadInput() {
		if(Input.GetKeyDown (KeyCode.Space)) {
			//TODO: Destroy any clones
			playerMovement.Rewind = true;
			current--;
			return;
		}
		if(Input.GetKeyUp (KeyCode.Space)) {
			playerMovement.Rewind = false;
			//TODO: Create a new clone
			return;
		}

		InputEvent inputEvent = null;
		if(Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor"))) {
				playerMovement.MoveTo(hit.point);
				inputEvent = new InputEvent(hit);
			}
		}
		recording.Add(inputEvent);
		current++;
	}
}
