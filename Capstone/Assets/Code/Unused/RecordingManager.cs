using UnityEngine;
using System.Collections.Generic;

public class RecordingManager : MonoBehaviour {
	public static RecordingManager Instance {
		get;
		private set;
	}

	private List<Recording> recordings = new List<Recording>();
	private List<CloneInput> recordedInput = new List<CloneInput>();
	public int NumHolograms { get { return recordedInput.Count; } }

	[SerializeField]
	private CloneInput recordedInputPrototype;

	void Awake() {
		//Instance = this;
	}

	public void OnRoundEnd() {
		foreach (CloneInput input in recordedInput) {
			if (input != null) {
				Destroy(input.gameObject);
			}
		}
		recordedInput.Clear();
		Transform playerTransform = GameObject.FindObjectOfType<PlayerInput>().transform;
		foreach (Recording recording in recordings) {
			CloneInput input = Instantiate(recordedInputPrototype, playerTransform.position, playerTransform.rotation) as CloneInput;
			//input.recording = recording;
			recordedInput.Add(input);
		}
		//Spawn Holograms
	}

	public void AddRecording(Recording recording) {
		recordings.Add(recording);
	}

	public void ClearRecordings() {
		recordings.Clear();
		foreach (CloneInput input in recordedInput) {
			if (input != null) {
				Destroy(input.gameObject);
			}
		}
		recordedInput.Clear();
	}
}
