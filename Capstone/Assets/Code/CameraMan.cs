using UnityEngine;
using System.Collections;

public class CameraMan : MonoBehaviour {

	public float MovementSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 direction = GetInputDirection();
		Vector3 to = new Vector3(transform.position.x + direction.x * MovementSpeed, 
		                         transform.position.y + direction.y * MovementSpeed, 
		                         transform.position.z);
		transform.position = to;
	}

	Vector2 GetInputDirection() {
		Vector2 direction = Vector2.zero;
		if (Input.GetKey(KeyCode.UpArrow)) {
			direction.y = 1;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			direction.y = -1;
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			direction.x = -1;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			direction.x = 1;
		}
		return direction.normalized;
	}
}
