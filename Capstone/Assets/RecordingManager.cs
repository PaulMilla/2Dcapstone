using UnityEngine;
using System.Collections.Generic;

public class RecordingManager : MonoBehaviour {
	public static RecordingManager Instance {
		get;
		private set;
	}

	private List<Recording> recordings = new List<Recording>();
	private List<RecordedInput> recordedInput = new List<RecordedInput>();
	[SerializeField]
	private RecordedInput recordedInputPrototype;

	void Awake() {
		Instance = this;
	}

	public void OnRoundEnd() {
		foreach (RecordedInput input in recordedInput) {
			if (input != null) {
				Destroy(input.gameObject);
			}
		}
		recordedInput.Clear();
		Transform playerTransform = GameObject.FindObjectOfType<PlayerInput>().transform;
		foreach (Recording recording in recordings) {
			RecordedInput input = Instantiate(recordedInputPrototype, playerTransform.position, playerTransform.rotation) as RecordedInput;
			input.recording = recording;
			recordedInput.Add(input);
		}
		//Spawn Holograms
	}
	public void AddRecording(Recording recording) {
		recordings.Add(recording);
	}
}
