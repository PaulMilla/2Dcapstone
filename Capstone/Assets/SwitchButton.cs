using UnityEngine;
using System.Collections;

public class SwitchButton : MonoBehaviour {

	// The list of doors that we will be calling OnSwitch on
	[SerializeField]
	Door[] doors;

	
	void OnTriggerStay(Collider other) {

		// If the collider is a player or hologram,
		// check to see if that player/hologram pressed the Interaction Button
		// during this frame
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

	// We call this ourselves in OnTriggerStay whenever a player activiates the switch
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
