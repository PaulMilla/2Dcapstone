using UnityEngine;
using System.Collections;

public class GuardKillMessage : MonoBehaviour {
	public string text;
	public int showCount;

	private float duration;
	private GameObject player;

	void Start () {
		duration = 5;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update() {
		float dist = Vector3.Distance(this.transform.position, player.transform.position);
		if(dist < 2.0f && showCount > 0) {
			UI.Dialog.ShowMessageForDuration(text, duration);
			--showCount;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag.Equals("Player")) {
			UI.Dialog.Hide();
		}
	}
}
