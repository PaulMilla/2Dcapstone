using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIView : MonoBehaviour {
	private float time { get { return GameManager.Instance.GameTime; } }

	[SerializeField]
	private Text timeText;
	[SerializeField]
	private Text hologramCountText;
	void Start() {

	}
	void Update() {
		if (time % 60 > 9) {
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
