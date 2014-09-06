using UnityEngine;
using System.Collections;

public class RecordedInput : MonoBehaviour
{
	private PlayerModel playerModel { get; set; }
	public Recording2 recording { get; set; }
	int iteration = 0;

	void Awake() {
		Debug.Log ("RecordedInput, Awake");
		playerModel = gameObject.GetComponent<PlayerModel>() as PlayerModel;
	}

	void FixedUpdate() {
		Debug.Log("RecordedInput, fixed update");
		Vector2 direction = GetInputDirection();
		playerModel.Move(direction);
		iteration++;
	}

	Vector2 GetInputDirection() {
		Vector2 direction = Vector2.zero;
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
		return direction.normalized;
	}
}

