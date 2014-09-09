using UnityEngine;
using System.Collections;

public class RecordedInput : MonoBehaviour
{
	private PlayerModel playerModel { get; set; }
	private bool interactionButtonDown;
	Vector2 direction = Vector2.zero;


	// Holds a list of recorded actions we can iterate through every frame
	public Recording recording { get; set; }

	// This keeps track of which frame we are on
	// it gets updated every FixedUpdate
	int iteration = 0;

	void Awake() {
		playerModel = gameObject.GetComponent<PlayerModel>() as PlayerModel;
	}
	
	void FixedUpdate() {
		if (GameManager.Instance.inRound) {
			GetInput();
			playerModel.Move(direction);
			iteration++;
		}
	}


	// The difference is here really Instead of using Input.GetKey, we can
	// use our recorded input for this frame.
	void GetInput() {
		direction = Vector2.zero;
		if (recording.GetKey(iteration, KeyCode.W)) {
			direction.y = 1;
		}
		if (recording.GetKey(iteration, KeyCode.S)) {
			direction.y = -1;
		}
		if (recording.GetKey(iteration, KeyCode.A)) {
			direction.x = -1;
		}
		if (recording.GetKey(iteration, KeyCode.D)) {
			direction.x = 1;
		}
		// By default this is false
		interactionButtonDown = false;
		if (recording.GetKeyDown(iteration, KeyCode.E)) {
			interactionButtonDown = true;
		}	
	}

	// A method which signifies whether or not this recording has pressed down the 
	// Interaction Button this frame
	public bool InteractionButtonDown() {
		return interactionButtonDown;
	}
}

