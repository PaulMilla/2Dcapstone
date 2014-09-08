using UnityEngine;
using System.Collections;

public class SwitchButton : MonoBehaviour {

	[SerializeField]
	Door[] doors;

	
	void OnTriggerStay(Collider other) {
		if (other.tag.Equals("Hologram") ||
		    other.tag.Equals("Player")) {


			if (other.tag.Equals("Hologram")) {
				RecordedInput hologram = other.transform.GetComponent<RecordedInput>();
				if (hologram.InteractionButtonDown()) {
					OnSwitch ();
				}
			}
			else if (other.tag.Equals("Player")) {
				PlayerInput player = other.transform.GetComponent<PlayerInput>();
				if (player.InteractionButtonDown()) {
					OnSwitch ();
				}
			}
		}
	}

	public void OnSwitch() {

		// Just a visual indicator
		if (renderer.material.color.Equals(Color.white))
			renderer.material.color = Color.yellow;
		else
			renderer.material.color = Color.white;

		// The reason this class exists
		foreach (Door door in doors) {
			door.Switch();
		}
	}
}
