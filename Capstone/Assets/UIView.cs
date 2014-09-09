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
		GUILayout.TextField("Holograms Remaining: " + (GameManager.Instance.HologramLimit - GameManager.Instance.NumHolograms));
	}
	void OnRoundStart() {
		TimeRoundStart = Time.time;
	}
}
