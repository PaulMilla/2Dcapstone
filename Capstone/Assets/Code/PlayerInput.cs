using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : CharacterInput {
	public GameObject hologramPrefab;
	protected PlayerMovement playerMovement;
	protected GameObject clone;

	void Awake() {
		playerMovement = GetComponent<PlayerMovement>();
	}

	void Update() {
		if(GameManager.Instance.inRound) {
			ReadInput();
		}
	}

	void ReadInput() {
		if(Input.GetKeyDown (rewindKey)) {
			playerMovement.Rewind = true;
		}

		if(Input.GetKeyUp (rewindKey)) {
			if(playerMovement.Rewind) {
				createClone(playerMovement.cloneEvents);
			}
			playerMovement.Rewind = false;
		}

		if(Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Interactable")))
            {
                playerMovement.MoveTo(hit);
            }
			else if(Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor"))) {
				playerMovement.MoveTo(hit);
			}
		}
	}

	void createClone(Stack<Event> events) {
		clone = GameObject.Instantiate(hologramPrefab, this.transform.position, this.transform.rotation) as GameObject;
		clone.GetComponent<CloneInput>().recordedInputs = events;
	}
}
