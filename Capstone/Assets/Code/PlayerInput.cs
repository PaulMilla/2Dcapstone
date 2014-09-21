using UnityEngine;
using System.Collections;

public class PlayerInput : CharacterInput {
	
	private Recording recording;
	
	public RecordedInput recordedInput;

	void Awake() {
		playerModel = gameObject.GetComponent<PlayerModel>() as PlayerModel;
	}

	void FixedUpdate() {
		if (GameManager.Instance.inRound) {
			GetInput();
		}
	}
	public void OnRoundStart() {
		recording = new Recording();
		RecordingManager.Instance.AddRecording(recording);
	}

	void GetInput() {
		RecordedEvent recordedEvent = new RecordedEvent ();
		if (Input.GetMouseButtonDown(0)) {
			recordedEvent.AddMouseButtonDown(0, Input.mousePosition);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor")))
			{
				playerModel.MoveTo(hit.point);
			}
		}
		recording.AddEvent (recordedEvent);
	}
}
