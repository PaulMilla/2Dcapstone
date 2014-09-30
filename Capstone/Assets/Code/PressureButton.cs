using UnityEngine;
using System.Collections;

public class PressureButton : MonoBehaviour {

	public bool Pressed = false;
	public float ButtonSpeed;

	// The y-dimesnion where the button is up and at rest
	float upRestY = .2f;
	// The y-dimension where the button is down and at rest
	float downRestY = -.2f;

	[SerializeField]
	private Activatable[] activatableArray;
	// Use this for initialization
	void Start () {
		ButtonSpeed = 1.0f;
		GameManager.Instance.RoundEnd += Reset;
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

		Pressed = false;
	}
	
	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag.Equals ("Player") || other.gameObject.tag.Equals ("Hologram")) {
			Pressed = true;
		}
	}


	void Reset() {
		Pressed = false;
	}
}
