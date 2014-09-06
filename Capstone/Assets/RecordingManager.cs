using UnityEngine;
using System.Collections.Generic;

public class RecordingManager : MonoBehaviour {
	public static RecordingManager Instance {
		get;
		private set;
	}

	private List<Recording> previousRecordings = new List<Recording>();
	private Recording currentRecroding;

	void Awake() {
		Instance = this;
		OnRoundEnd();
	}

	public void OnRoundEnd() {
		if (currentRecroding != null) {
			previousRecordings.Add(currentRecroding);
		}
		currentRecroding = new Recording();
	}
	public void RecordMovement(Vector2 direction) {
		currentRecroding.RecordMovement(direction);
	}
}
