using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	private PlayerModel playerModel { get; set; }

	void Awake() {
		playerModel = gameObject.GetComponent<PlayerModel>() as PlayerModel;
	}
	void FixedUpdate() {
			Vector2 direction = GetInputDirection();
			RecordingManager.Instance.RecordMovement(direction);
			playerModel.Move(direction);
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
		return direction.normalized;
	}
}
