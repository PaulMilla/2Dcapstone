using UnityEngine;
using System.Collections.Generic;

public class RecordingManager : MonoBehaviour {
	public static RecordingManager Instance {
		get;
		private set;
	}

	private List<Recording> recordings = new List<Recording>();

	void Awake() {
		Instance = this;
		OnRoundStart();
	}

	public void OnRoundStart() {
		//Spawn Holograms
	}
	public void AddRecording(Recording recording) {
		recordings.Add(recording);
	}
}
