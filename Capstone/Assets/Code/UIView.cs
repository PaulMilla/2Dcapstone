using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIView : MonoBehaviour {
	private float time { get; set; }

	[SerializeField]
	private Text timeText;
	[SerializeField]
	private Text hologramCountText;
	void Start() {

	}
	void Update() {
		if (Input.GetKey(KeyCode.R)) {
			time -= Time.deltaTime;
			if (time < 0) {
				time = 0;
			}
		}
		else {
			time += Time.deltaTime;
		}
		if (time % 60 > 10) {
			timeText.text = (int)time / 60 + ":" + (int)time % 60;
		}
		else {
			timeText.text = (int)time / 60 + ":0" + (int)time % 60;
		}
		if (Input.GetKeyUp(KeyCode.R)) {
			hologramCountText.text = ""+1;
		}
	}
}
