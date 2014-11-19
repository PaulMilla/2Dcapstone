using UnityEngine;
using System.Collections;

public static class GameState {

	public static bool Paused {
		set {
			_isPaused = value;
			if (value) {
				if (StartPaused != null) 
					StartPaused();
			}
			else {
				if (EndPaused != null) 
					EndPaused();
			}
		}
		get {
			return _isPaused;
		}
	}
	private static bool _isPaused;

	public static PlayerMovement.PlayerEvent StartPaused;
	public static PlayerMovement.PlayerEvent EndPaused;
}
