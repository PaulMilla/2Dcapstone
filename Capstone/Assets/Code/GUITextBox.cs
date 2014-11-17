using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUITextBox : MonoBehaviour {
    private Text text;
    private Animator dialog;
	private bool isTimed;
    private float showTime;
	public bool clickToGoAway {get;set;}

	// Use this for initialization
	void Start () {
        dialog = GetComponent<Animator>();
        text = GetComponentInChildren<Text>();
		isTimed = false;
        showTime = 0.0f;
	}

	public void ShowMessage(string message) {
		if (text == null) return;
		text.text = message;
		Show();
	}

    public void ShowMessageForDuration(string message, float duration) {
		if (text == null) return;
        text.text = message;
        showTime = duration;
		isTimed = true;
        if (showTime > 0.0f) {
			Show();
        }
    }

	public void Clicked() {
		if (clickToGoAway) 
			Hide();
	}

	public void Show() {
		dialog.Play("GUITextBox_Show");
	}

	public void Hide() {
		dialog.Play("GUITextBox_Hide");
	}

    void FixedUpdate() {
        if (isTimed && showTime > 0.0f) {
            showTime -= Time.fixedDeltaTime;
            if (showTime <= 0.0f) {
				Hide();
				isTimed = false;
            }
        }
    }
}
