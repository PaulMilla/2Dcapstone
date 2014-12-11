using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {
	public GameObject leftFootstepPrefab;
	public GameObject rightFootstepPrefab;

	private Vector3 lastFootstep;
	private bool isRewinding;

	AudioSource audioFootstep;
		// Use this for initialization
	void Start () {
		audioFootstep = this.transform.FindChild ("Sound").FindChild ("Audio_Footsteps").GetComponent<AudioSource> ();
		lastFootstep = Vector3.zero;
		isRewinding = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Rewind")) {
			isRewinding = true;
		}
		if(Input.GetButtonUp("Rewind")) {
			isRewinding = false;
		}
	}

	// Called from animation Event
	void PlayFootstepSound() {
		audioFootstep.pitch = Random.Range (0.6f, 1.0f);;
		audioFootstep.Play ();
	}

	void LeftFootstepDown() {
		Vector3 newPosition = this.transform.position;
		Quaternion newRotation = this.transform.rotation;
		newRotation.eulerAngles = new Vector3(90, newRotation.eulerAngles.y, newRotation.eulerAngles.z);

		GameObject.Instantiate(leftFootstepPrefab, newPosition, newRotation);
		lastFootstep = newPosition;
		PlayFootstepSound();
	}

	void RightFootstepDown() {
		Vector3 newPosition = this.transform.position;
		Quaternion newRotation = this.transform.rotation;
		newRotation.eulerAngles = new Vector3(90, newRotation.eulerAngles.y, newRotation.eulerAngles.z);

		GameObject.Instantiate(rightFootstepPrefab, newPosition, newRotation);
		lastFootstep = newPosition;
		PlayFootstepSound();
	}

	void FixedUpdate() {
		if (isRewinding) {
			return;
		}
	}
}