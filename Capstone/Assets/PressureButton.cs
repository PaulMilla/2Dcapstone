using UnityEngine;
using System.Collections;

public class PressureButton : MonoBehaviour {

	public bool Pressed = false;
	public float ButtonSpeed;

	// The y-dimesnion where the button is up and at rest
	float upRestY = .2f;
	// The y-dimension where the button is down and at rest
	float downRestY = -.2f;

	public Door Door;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = transform.position;
		if (!Pressed && transform.position.y < upRestY) {
			// Move down
			newPosition.y += ButtonSpeed * Time.fixedDeltaTime;
		}
		else if (Pressed && transform.position.y > downRestY) {
			// Move up
			newPosition.y -= ButtonSpeed * Time.fixedDeltaTime;
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
