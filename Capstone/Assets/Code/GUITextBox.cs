using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUITextBox : MonoBehaviour {
    Text text;
    Animator dialog;
    float hideTime;
    float HideTime {
        get { return hideTime; }
        set {
            hideTime = value;
            if (hideTime <= 0) {
                hideTime = 0.0f;
                Hide();
            }
        }
    }

	// Use this for initialization
	void Start () {
        dialog = GetComponent<Animator>();
        text = GetComponentInChildren<Text>();
	}

    void FixedUpdate() {
        if (HideTime > 0) {
            HideTime = HideTime - Time.fixedDeltaTime;
        }
    }

    public void Show() {
		dialog.SetTrigger("Show");
    }

    public void Hide() {
		dialog.SetTrigger("Hide");
    }

    public void HideAfter(float f) {
        HideTime = f;
    }

    public void ChangeText(string s) {
        text.text = s;
    }
}
