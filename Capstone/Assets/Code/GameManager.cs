using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get;
		private set;
	}
	public delegate void GameEvent();

	public GameEvent RoundStart;
	public GameEvent RoundEnd;
	public GameEvent LevelFailed;

	[SerializeField]
	private string nextLevelName;

	[SerializeField]
	private int hologramLimit;
	public int HologramLimit { get { return hologramLimit; } private set { hologramLimit = value; } }
	public int HologramsRemaining { get { return HologramLimit - RecordingManager.Instance.NumHolograms; } } // NYI
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
		if (Input.GetKeyUp(KeyCode.Space)) {
			if (inRound) {
				//EndRound();
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
		RecordingManager.Instance.ClearRecordings();
	}
	public void BeginRound() {
		playerInput.OnRoundStart();
		inRound = true;
		if (RoundStart != null) {
			RoundStart();
		}
		foreach (PlayerMovement playerModel in FindObjectsOfType<PlayerMovement>()) {
			playerModel.movementEnabled = true;
		}
	}
	public void EndRound() {
		inRound = false;
		foreach (var point in spawnPoints) {
			point.DestroySpawnedObject();
			point.Spawn();
		}
		if (RoundEnd != null) {
			RoundEnd();
		}

		playerInput = GameObject.FindObjectOfType<PlayerInput>();
		RecordingManager.Instance.OnRoundEnd();
		if (HologramsRemaining == 0) {
			LevelFailed();
			RecordingManager.Instance.ClearRecordings();
		}
		foreach (PlayerMovement playerMovement in FindObjectsOfType<PlayerMovement>()) {
			playerMovement.movementEnabled = false;
		}
	}
}
