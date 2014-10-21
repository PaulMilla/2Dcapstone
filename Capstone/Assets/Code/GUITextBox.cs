using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUITextBox : MonoBehaviour {
    private static Animator dialog;
    private static Text text;
    private static float showTime;

	// Use this for initialization
	void Start () {
        dialog = GetComponent<Animator>();
        text = GetComponentInChildren<Text>();
        showTime = 0.0f;
	}
	
    public static void ShowMessageForDuration(string message, float duration) {
        text.text = message;
        showTime = duration;
        if (showTime > 0.0f) {
            Show();
        }
    }

    public static void Hide() {
        dialog.Play("GUITextBox_Hide");
    }

    public static void Show() {
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
