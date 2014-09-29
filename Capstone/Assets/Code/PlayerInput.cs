using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : CharacterInput {
	public GameObject hologram;
	protected PlayerMovement playerMovement;

	public void OnRoundStart() {
		playerMovement = GetComponent<PlayerMovement>();
	}

	void Awake() {
		playerMovement = GetComponent<PlayerMovement>();
	}

	void Update() {
		if(GameManager.Instance.inRound) {
			ReadInput();
		}
	}

	void ReadInput() {
		if(Input.GetKeyDown (KeyCode.Space)) {
			//TODO: Destroy any clones
			playerMovement.Rewind = true;
		}

		if(Input.GetKeyUp (KeyCode.Space)) {
			playerMovement.Rewind = false;
			createClone(playerMovement.cloneEvents);
		}

		if(Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor"))) {
				playerMovement.MoveTo(hit.point);
			}
		}
	}

	void createClone(Stack<Event> events) {
		GameObject clone = GameObject.Instantiate(hologram, this.transform.position, this.transform.rotation) as GameObject;
		clone.GetComponent<CloneInput>().SetEvents(events);
	}
}
