using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {
	public GameObject footstepPrefab;
	public float footstepDistances;

	private Vector3 lastFootstep;
	private bool isRewinding;

	// Use this for initialization
	void Start () {
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

	void FixedUpdate() {
		if (isRewinding) {
			return;
		}

		if (Vector3.Distance(lastFootstep, transform.position) > footstepDistances) {
			Vector3 newPosition = this.transform.position;
			Quaternion newRotation = this.transform.rotation;
			newRotation.eulerAngles = new Vector3(90, newRotation.eulerAngles.y, newRotation.eulerAngles.z);

			GameObject.Instantiate(footstepPrefab, newPosition, newRotation);
			lastFootstep = newPosition;
		}
	}
}