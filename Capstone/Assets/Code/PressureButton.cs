﻿using UnityEngine;
using System.Collections;

public class PressureButton : MonoBehaviour {

	public bool Pressed = false;
	public float ButtonSpeed;

	// If the player is not detected to be on the button for X number of frames,
	// Deactivate the button 
	private int framesSinceLastDetected = 0;
	private int frameResetThreshold = 3;

	// The y-dimesnion where the button is up and at rest
	float upRestY = 0f;
	// The y-dimension where the button is down and at rest
	float downRestY = -.15f;

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

		if (Pressed) {
			foreach (Activatable activatable in activatableArray) {
				activatable.Activate ();
			}
		} else {
			foreach (Activatable activatable in activatableArray) {
				activatable.Deactivate ();
			}
		}

		framesSinceLastDetected++;
		if (framesSinceLastDetected > 3) {
				Pressed = false;
		}
	}
	
	void OnTriggerStay(Collider other) {
		StartCoroutine(myOnTriggerStay (other));
	}

	private IEnumerator myOnTriggerStay(Collider other) {
		Debug.Log ("OK");
		yield return new WaitForEndOfFrame();
		if (other.gameObject.tag.Equals ("Player") || other.gameObject.tag.Equals ("Hologram")) {
			framesSinceLastDetected = 0;
			Pressed = true;
		}
	}


	void Reset() {
		Pressed = false;
	}
}
