using UnityEngine;
using System.Collections;

public class SoundManagerLevel1 : MonoBehaviour {

	AudioSource current;

	public AudioSource welcome;
	public AudioSource alert;

	public float time_;

	bool triggered = false;

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

	void OnTriggerEnter(Collider other) {
		if (triggered) {
			return;
		}
		if (other.tag.Equals("Player")) {
			triggered = true;
			if (current != null) {
				current.Stop();
				current = alert;
				alert.Play();
			}
		}
	}

	void Pause() {
		if (current != null) {
			current.Pause();
		}
	}

	void Resume() {
		if (current != null) {
			current.Play();
		}
	}

}
