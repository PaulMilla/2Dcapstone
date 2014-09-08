using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	private PlayerModel playerModel { get; set; }
	private Recording recording;

	// Set to true for a frame whenever interaction button is down
	private bool interactionButtonDown = false;

	Vector2 direction = Vector2.zero;

	public RecordedInput recordedInput;

	void Awake() {
		playerModel = gameObject.GetComponent<PlayerModel>() as PlayerModel;
	}

	void FixedUpdate() {
		if (GameManager.Instance.inRound) {
			GetInput();
			playerModel.Move(direction);
		}
	}
	public void OnRoundStart() {
		recording = new Recording();
		RecordingManager.Instance.AddRecording(recording);
	}

	// Here we can create a new RecordedEvent which will be similar to 
	// an InputEvent, after we update it based on what is going on during 
	// the current FixedUpdate, we add it to our recording.
	void GetInput() {
		direction = Vector2.zero;
		RecordedEvent recordedEvent = new RecordedEvent();
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
		// By default this is false
		interactionButtonDown = false;
		if (Input.GetKeyDown(KeyCode.E)) {
			interactionButtonDown = true;
			recordedEvent.AddKeyDown(KeyCode.E);
		}
		recording.AddEvent(recordedEvent);
	}

	// A method which signifies whether or not this recording has pressed down the 
	// Interaction Button this frame
	public bool InteractionButtonDown() {
		return interactionButtonDown;
	}
}
