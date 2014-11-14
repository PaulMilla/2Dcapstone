using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUITextBox : MonoBehaviour {
    private Animator dialog;
    private Text text;
    private float showTime;

	// Use this for initialization
	void Start () {
        dialog = GetComponent<Animator>();
        text = GetComponentInChildren<Text>();
        showTime = 0.0f;
	}
	
    public void ShowMessageForDuration(string message, float duration) {
		if (text == null) return;
        text.text = message;
        showTime = duration;
        if (showTime > 0.0f) {
            Show();
        }
    }

    public void Hide() {
        dialog.Play("GUITextBox_Hide");
    }

    public void Show() {
        dialog.Play("GUITextBox_Show");
    }

    void FixedUpdate() {
        if (showTime > 0.0f) {
            showTime -= Time.fixedDeltaTime;
            if (showTime <= 0.0f) {
                Hide();
            }
        }
    }
}
