using UnityEngine;
using System.Collections;

public class UIView : MonoBehaviour {
	private float TimeRoundStart { get; set; }
	void Start() {
		GameManager.Instance.RoundStart += OnRoundStart;
	}
	void OnGUI() {
		if (GameManager.Instance.inRound) {
			GUILayout.TextField("Time: "+(Time.time - TimeRoundStart));
		}
		else {
			GUILayout.TextField("Time: "+0);
		}
		GUILayout.TextField("Holograms Remaining: " + (GameManager.Instance.HologramsRemaining - 1));
		if (GameManager.Instance.inRound) {
			if (GUILayout.Button("End Round")) {
				GameManager.Instance.EndRound();
			}
		}
		else {
			if (GUILayout.Button("Begin Round")) {
				GameManager.Instance.BeginRound();
			}
		}
		if (GUILayout.Button("Reset Level")) {
			GameManager.Instance.ResetLevel();
		}
	}
	void OnRoundStart() {
		TimeRoundStart = Time.time;
	}
}
