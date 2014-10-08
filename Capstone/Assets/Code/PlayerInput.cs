using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {
	public UnitModel hologramPrefab;

	protected Recording recording;

	void Start() {
		recording = new Recording(gameObject);
		GameManager.Instance.AddRecording(recording);
		GameManager.Instance.RewindEvent += () => { if (!GameManager.Instance.IsRewinding) recording.Split(hologramPrefab);};
	}

	void Update() {
		ReadInput();
	}

	void ReadInput() {
		if(Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Interactable")))
            {
				Debug.LogError("Interactable not yet implemeneted");
            }
			else if(Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor"))) {
				recording.AddEvent(new RecordedEvent(hit.point, transform.position, GameManager.Instance.GameTime, EventType.Move));
			}
		}
	}
}
