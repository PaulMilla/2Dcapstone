using UnityEngine;
using System.Collections;

public class SystemEvent : MonoBehaviour {
    public string text = "Insert Text Here!";
	public bool clickToGoAway = true;
	public bool keepActivating = false;
	public int ActivateCount = 1;
	public KeyCode closeOnButton;

	void Start() {
		//base.Start();
	}

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
		UI.System.clickToGoAway = clickToGoAway;
		UI.System.ShowMessage(text);
	}

	void Deactivate() {
		UI.System.Hide();
	}
}
