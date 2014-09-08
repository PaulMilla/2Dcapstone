using UnityEngine;
using System.Collections;

public class PressureButton : MonoBehaviour {

	public bool Pressed = false;
	public float ButtonSpeed;

	public Door Door;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = transform.position;
		if (!Pressed && transform.position.z > -.25) 
			newPosition.z -= ButtonSpeed;
		else if (Pressed && transform.position.z < .1) {
			newPosition.z += ButtonSpeed;
		}
		transform.position = newPosition;
	}

	public void activate ()
	{
		Door.Open();
		Pressed = true;
	}

	public void deactivate ()
	{
		Door.Close();
		Pressed = false;
	}

	void OnTriggerEnter(Collider other) {
		if(!Pressed) {
			if (other.gameObject.tag.Equals("Player") || 
			    other.gameObject.tag.Equals("Hologram")) {
				activate();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(Pressed) {
			if (other.gameObject.tag.Equals("Player") || 
			    other.gameObject.tag.Equals("Hologram")) {
				deactivate();
			}
		}
	}
}
