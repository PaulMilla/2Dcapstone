using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	private PlayerModel playerModel { get; set; }
	private Recording2 recording = new Recording2();
	public RecordedInput hologram;

	void Awake() {
		playerModel = gameObject.GetComponent<PlayerModel>() as PlayerModel;
	}
	void FixedUpdate() {
		Vector2 direction = GetInputDirection();
		RecordingManager.Instance.RecordMovement(direction);
		playerModel.Move(direction);
		Record();
	}

	// Here we can create a new RecordedEvent which will be similar to 
	// an InputEvent, after we update it based on what is going on during 
	// the current FixedUpdate, we add it to our recording.
	void Record() {
		RecordedEvent recordedEvent = new RecordedEvent();
		if (Input.GetKey(KeyCode.W)) {
			Debug.Log ("Adding 'W'");
			recordedEvent.AddKey(KeyCode.W);
		}
		if (Input.GetKey(KeyCode.S)) {
			recordedEvent.AddKey(KeyCode.S);
		}
		if (Input.GetKey(KeyCode.A)) {
			recordedEvent.AddKey(KeyCode.A);
		}
		if (Input.GetKey(KeyCode.D)) {
			Debug.Log ("Adding 'D'");
			recordedEvent.AddKey(KeyCode.D);
		}
		recording.AddEvent(recordedEvent);
	}

	Vector2 GetInputDirection() {
		Vector2 direction = Vector2.zero;
		if (Input.GetKey(KeyCode.W)) {
			direction.y = 1;
		}
		if (Input.GetKey(KeyCode.S)) {
			direction.y = -1;
		}
		if (Input.GetKey(KeyCode.A)) {
			direction.x = -1;
		}
		if (Input.GetKey(KeyCode.D)) {
			direction.x = 1;
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			// Create a new hologram with recordedInput as controller
			RecordedInput hologramCharacter = (RecordedInput) Instantiate(hologram, new Vector3(0, 0, this.transform.position.z), transform.rotation);
			Debug.Log ("Creating hologram character with " + recording.NumEvents() + " events ");
			hologramCharacter.recording = recording;
			recording = new Recording2();
		}
		return direction.normalized;
	}
}
