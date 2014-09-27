using UnityEngine;
using System.Collections;

public class InputEvent {
	public RaycastHit hit {get; set;}

	public InputEvent(RaycastHit _hit) {
		hit = _hit;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
