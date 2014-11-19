using UnityEngine;
using System.Collections;

public class TextEvent : MonoBehaviour {
    public string text = "Insert Text Here!";
	public bool clickToGoAway = true;
	public bool keepActivating = false;
	public int ActivateCount = 1;
	public KeyCode closeOnButton = KeyCode.Escape;
	public bool isTimed = true;
    public float duration;

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player") && (--ActivateCount >= 0 || keepActivating)) {
			Activate();
        }
    }

	void Update() {
		if(Input.GetKeyDown(closeOnButton)) {
			Deactivate();
		}
	}

	void Activate() {
		UI.Dialog.clickToGoAway = clickToGoAway;
		if(isTimed && duration > 0) {
			UI.Dialog.ShowMessageForDuration(text, duration);
		} else {
			UI.Dialog.ShowMessage(text);
		}
	}

	void Deactivate() {
		UI.Dialog.Hide();
	}
}
