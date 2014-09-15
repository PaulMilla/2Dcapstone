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
		if (!Pressed && transform.position.y > +.25) {
			// Move down
			newPosition.y -= ButtonSpeed;
		}
		else if (Pressed && transform.position.y < -.1) {
			// Move up
			newPosition.y += ButtonSpeed;
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
