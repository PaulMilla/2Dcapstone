using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundControlUIElement : MonoBehaviour {
	[SerializeField]
	Text text;
	[SerializeField]
	Text subText;

	//int displayForSeconds = 1;
	// Use this for initialization
	void Start () {
		text.fontSize = 50;
		text.text = "LEVEL START";
		subText.text = "Press space to begin first round";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnRoundEnd() {
		text.fontSize = 50;
		text.text = "ROUND END";
		subText.text = "Press space to begin next round";
		gameObject.SetActive(true);
	}
	void OnRoundStart() {
		gameObject.SetActive(false);
	}
	void OnLevelFailed() {
		text.fontSize = 26;
		text.text = "LEVEL FAILED (Out of holograms)";
		subText.text = "Press space to retry";
		gameObject.SetActive(true);
	}
}
