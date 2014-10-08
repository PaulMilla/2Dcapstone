using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get;
		private set;
	}
	public delegate void GameEvent();
	public GameEvent RewindEvent;

	[SerializeField]
	private string nextLevelName;
	[SerializeField]
	private int hologramLimit;

	public float GameTime { get; private set; }
	public bool IsRewinding { get; private set; }
	private List<Recording> RecordingList { get; set; }

	void Awake() {
		Instance = this;
		RecordingList = new List<Recording>();
	}

	void Start() {

	}
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			IsRewinding = true;
			RewindEvent();
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			IsRewinding = false;
			RewindEvent();
		}
	}
	void FixedUpdate() {
		if (!IsRewinding) {
			GameTime += Time.fixedDeltaTime;
		}
		else {
			GameTime = Mathf.Max(0, GameTime - Time.fixedDeltaTime);
		}
		foreach (Recording recording in RecordingList) {
			recording.FixedUpdate();
		}
	}
	public void AddRecording(Recording recording) {
		RecordingList.Add(recording);
	}
	public void LoadNextLevel() {
		Application.LoadLevel(nextLevelName);
	}
}
