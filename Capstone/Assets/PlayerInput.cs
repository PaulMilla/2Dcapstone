using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	private PlayerModel playerModel { get; set; }
	private Recording recording = new Recording();
	public RecordedInput hologram;

	void Awake() {
		playerModel = gameObject.GetComponent<PlayerModel>() as PlayerModel;
	}
	void FixedUpdate() {
		Vector2 direction = GetInputDirection();
		playerModel.Move(direction);
	}

	// Here we can create a new RecordedEvent which will be similar to 
	// an InputEvent, after we update it based on what is going on during 
	// the current FixedUpdate, we add it to our recording.
	Vector2 GetInputDirection() {
		RecordedEvent recordedEvent = new RecordedEvent();
		Vector2 direction = Vector2.zero;
		if (Input.GetKey(KeyCode.W)) {
			direction.y = 1;
			recordedEvent.AddKey(KeyCode.W);
		}
		if (Input.GetKey(KeyCode.S)) {
			direction.y = -1;
			recordedEvent.AddKey(KeyCode.S);
		}
		if (Input.GetKey(KeyCode.A)) {
			direction.x = -1;
			recordedEvent.AddKey(KeyCode.A);
		}
		if (Input.GetKey(KeyCode.D)) {
			direction.x = 1;
			recordedEvent.AddKey(KeyCode.D);
		}
		recording.AddEvent(recordedEvent);
		if (Input.GetKeyDown(KeyCode.Space)) {
			RecordingManager.Instance.AddRecording(recording);
			recording = new Recording();
		}
		return direction.normalized;
	}
}
