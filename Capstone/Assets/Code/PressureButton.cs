using UnityEngine;
using System.Collections;

public class PressureButton : MonoBehaviour {

	private bool _pressed;
	private bool Pressed { 
		get {return _pressed; } 
		set {
			if (_pressed != value) {
				foreach (Activatable activatable in activatableArray) {
					activatable.Toggle();
				}
			}
			_pressed = value;
		}
	}
	public float ButtonSpeed;

	// If the player is not detected to be on the button for X number of frames,
	// Deactivate the button 
	private int framesSinceLastDetected = 0;
	//private int frameResetThreshold = 3;

	// The y-dimesnion where the button is up and at rest
	float upRestY = 0.1f;
	// The y-dimension where the button is down and at rest
	float downRestY = -.05f;

	[SerializeField]
	private Activatable[] activatableArray;

	public Transform connectorsParent;
	private Transform[] connectors;
	private Color pressedColor =  new Color(.15625f, .89843f, .27343f, 1f);
	private Color unpressedColor = new Color(.15625f, .78125f, .95703f, 0.75f);

	// Use this for initialization
	void Start () {
		ButtonSpeed = 1.0f;
		GameManager.Instance.RoundEnd += Reset;
		if (connectorsParent != null) {
			connectors = connectorsParent.GetComponentsInChildren<Transform> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = transform.position;
		if (!Pressed && transform.position.y < upRestY) {
			if ((transform.position.y + ButtonSpeed * Time.fixedDeltaTime) > upRestY) {
				newPosition.y = upRestY;
			} else {
				newPosition.y += ButtonSpeed * Time.fixedDeltaTime;
			}
		}
		else if (Pressed && transform.position.y > downRestY) {
			if ((transform.position.y - ButtonSpeed * Time.fixedDeltaTime) > downRestY) {
				newPosition.y = downRestY;
			} else {
				newPosition.y -= ButtonSpeed * Time.fixedDeltaTime;
			}
		}
		transform.position = newPosition;

		framesSinceLastDetected++;
		if (framesSinceLastDetected > 3) {
				Pressed = false;
				ChangeColor(unpressedColor);
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Hologram")
		    || other.gameObject.tag.Equals("Enemy") ) {
			framesSinceLastDetected = 0;
			Pressed = true;

			ChangeColor(pressedColor);
		}
		//StartCoroutine(myOnTriggerStay (other));
	}
	void Reset() {
		Pressed = false;
	}

	void ChangeColor(Color c) {
		this.renderer.material.color = c;
		if (connectors != null) {
			foreach (Transform connector in connectors) {
				if (connector.renderer != null) {
					connector.renderer.material.color = c;							
				}
			}			
		}
	}
}
