using UnityEngine;
using System.Collections;

public class Event {
	public RaycastHit hit;
	public Vector3 target;
	public Vector3 position;
	public bool interacting;
	public Quaternion rotation; //TODO: Implement rotation

	public Event(Vector3 _position, bool _interacting, Vector3 _target) {
		position = _position;
		interacting = _interacting;
		target = _target;
		//rotation = _rotation;
	}
}
