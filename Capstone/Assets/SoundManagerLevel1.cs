using UnityEngine;
using System.Collections;

public class SoundManagerLevel1 : MonoBehaviour {

	AudioSource current;

	public AudioSource welcome;
	public AudioSource alert;

	public float time_;

	// Use this for initialization
	void Start () {
		current = welcome;
	}

	float rewindTime = 0.0f;
	float stopTime;
	bool rewinding = false;

	// Update is called once per frame
	void Update () {
		if (current != null && current.isPlaying) {
			if (Input.GetKeyDown(KeyCode.R)) {
				current.pitch = -1f;
			} else if (Input.GetKeyUp(KeyCode.R)) {
				current.pitch = 1f;
			} else if (Input.GetKey (KeyCode.R)) {
				if (current.time <= 0.1f) {
					current.pitch = 0f;
				}
			}
			time_ = current.time;
		}
	}

	void StartClock() {

	}

	void StopClock() {

	}
}
