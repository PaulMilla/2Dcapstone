using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {
	public GameObject hologramPrefab;
	protected PlayerMovement playerMovement;
	protected GameObject clone;

	AudioSource audioRewindLoop;
	AudioSource audioRewindBegin;
	AudioSource audioRewindEnd;

	void Awake() {
		playerMovement = GetComponent<PlayerMovement>();
		audioRewindLoop = transform.Find ("Sound").Find("Audio_Rewind_Loop").gameObject.GetComponent<AudioSource> ();
		audioRewindBegin = transform.Find ("Sound").Find ("Audio_Rewind_Begin").gameObject.GetComponent<AudioSource> ();
		audioRewindEnd = transform.Find ("Sound").Find ("Audio_Rewind_End").gameObject.GetComponent<AudioSource> ();
		playerMovement.RewindEnd = () => {
			audioRewindEnd.Play();
			audioRewindLoop.Stop();
			createClone(playerMovement.cloneEvents);
		};
	}

	void Update() {
		if(GameManager.Instance.inRound) {
			ReadInput();
		}
	}

	void ReadInput() {
		if(Input.GetButtonDown("Rewind")) {
			playerMovement.Rewind = true;
			audioRewindBegin.Play();
			audioRewindLoop.Play();
		}

		if(Input.GetButtonUp("Rewind")) {
			playerMovement.Rewind = false;
		}

		if (!playerMovement.Rewind) {
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Interactable"))) {
					playerMovement.MoveTo(hit, true);
				}
			}
			else if (Input.GetMouseButton(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Interactable"))) {
					playerMovement.MoveTo(hit);
				}
				else if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Floor"))) {
					playerMovement.MoveTo(hit);
				}
			}
		}
	}

	void createClone(Stack<Event> events) {
		clone = GameObject.Instantiate(hologramPrefab, this.transform.position, this.transform.rotation) as GameObject;
		clone.GetComponent<CloneInput>().recordedInputs = events;
	}
}
