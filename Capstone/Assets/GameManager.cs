using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get;
		private set;
	}

	[SerializeField]
	private string nextLevelName;
	public bool inRound { get; private set; } // true if clock is running and player's actions are currently happening
	private SpawnPoint[] spawnPoints { get; set; }
	private PlayerInput playerInput { get; set; }

	void Awake() {
		Instance = this;
	}

	void Start() {
		spawnPoints = FindObjectsOfType<SpawnPoint>();
		foreach (var point in spawnPoints) {
			point.Spawn();
		}
		playerInput = GameObject.FindObjectOfType<PlayerInput>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (inRound) {
				EndRound();
			}
			else {
				BeginRound();
			}
		}
	}

	public void LoadNextLevel() {
		Application.LoadLevel(nextLevelName);
	}
	public void ResetLevel() {
		EndRound();
		//TODO: RecordingManager.ClearRecordings();
	}
	public void BeginRound() {
		playerInput.OnRoundStart();
		inRound = true;
	}
	public void EndRound() {
		inRound = false;
		foreach (var point in spawnPoints) {
			point.DestroySpawnedObject();
			point.Spawn();
		}
		playerInput = GameObject.FindObjectOfType<PlayerInput>();
		RecordingManager.Instance.OnRoundEnd();
	}
}
