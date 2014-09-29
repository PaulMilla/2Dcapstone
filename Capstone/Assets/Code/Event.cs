using UnityEngine;
using System.Collections;

public class Event {
	public RaycastHit target {get; private set;}
	public Vector3 position {get; private set;}
	public Quaternion rotation {get; private set;}

	public Event(RaycastHit hit, Transform current) {
		this.target = hit;
		this.position = current.position;
		this.rotation = current.rotation;
	}
}
