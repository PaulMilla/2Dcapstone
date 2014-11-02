using UnityEngine;
using System.Collections;

public class SoundMananger : MonoBehaviour {

	private AudioSource levelMusic;
	private AudioSource currentMusic;

	// Use this for initialization
	void Start () {
		levelMusic = this.GetComponent<AudioSource> ();
		currentMusic = levelMusic;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Rewind")) {
			if (currentMusic != null) {
				currentMusic.pitch = -1;
			}
		} else if (Input.GetButtonUp("Rewind")) {
			if (currentMusic != null) {
				currentMusic.pitch = 1;
			}
		}
	}
}
