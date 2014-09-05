using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	[SerializeField]
	private float timeBetweenUpdates;
	private float timeSinceUpdate { get; set; }

	private PlayerModel playerModel { get; set; }

	void Awake() {
		playerModel = gameObject.GetComponent<PlayerModel>() as PlayerModel;
	}
	void FixedUpdate() {
		if (Time.time > timeBetweenUpdates + timeSinceUpdate) {
			timeSinceUpdate = Time.deltaTime;
			Vector2 direction = GetInputDirection();
			playerModel.Move(direction);
		}
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
