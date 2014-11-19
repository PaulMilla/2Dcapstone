using UnityEngine;
using System.Collections;

public class Voice : MonoBehaviour {

	public AudioSource voice;

	void Awake() {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Rewind")) {
			if (voice != null) {
				voice.pitch = -1;
			}
		} else if (Input.GetButtonUp("Rewind")) {
			if (voice != null) {
				voice.pitch = 1;
			}
		}
	}
}
