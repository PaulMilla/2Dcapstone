using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIView : MonoBehaviour {
	[SerializeField]
	private Text timer;
	void Start() {
		
	}
	void Update() {
		if (Time.time % 60 < 10) {
			timer.text = (int)Time.time / 60 + ":0" + (int)Time.time % 60;
		}
		else {
			timer.text = (int)Time.time / 60 + ":" + (int)Time.time % 60;
		}
	}
	void OnRoundStart() {
	}
}
