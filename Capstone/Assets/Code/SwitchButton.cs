using UnityEngine;
using System.Collections;

public class SwitchButton : MonoBehaviour {

	// The list of doors that we will be calling OnSwitch on
	[SerializeField]
	Activatable[] activatableArray;

	void Start() {
		GameManager.Instance.RoundEnd += Reset;
	}
	// Should be something like OnInteract()
	public void OnSwitch() {
		// Just a visual indicator
		if (renderer.material.color.Equals(Color.white))
			renderer.material.color = Color.yellow;
		else
			renderer.material.color = Color.white;

		// The reason this class exists
		foreach (Activatable activatable in activatableArray) {
			activatable.Toggle();
		}
	}
	void Reset() {

	}
}
