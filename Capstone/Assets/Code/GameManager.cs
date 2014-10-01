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
		BeginRound();
	}

	void Update() {
	}

	public void LoadNextLevel() {
		Application.LoadLevel(nextLevelName);
	}
	public void ResetLevel() {
		EndRound();
	}
	public void BeginRound() {
		inRound = true;
		if (RoundStart != null) {
			RoundStart();
		}
		foreach (PlayerMovement playerModel in FindObjectsOfType<PlayerMovement>()) {
			playerModel.movementEnabled = true;
		}
	}
	public void EndRound() {

	}
}
