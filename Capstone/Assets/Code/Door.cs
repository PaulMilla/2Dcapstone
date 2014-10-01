using UnityEngine;
using System.Collections;

public class Door : Activatable {



	protected override void Activate() {
		renderer.enabled = false;
		collider.enabled = false;
	}

	protected override void Deactivate() {
		renderer.enabled = true;
		collider.enabled = true;
	}
}
