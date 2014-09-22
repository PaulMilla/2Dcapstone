using UnityEngine;
using System.Collections;

public class Door : Activatable {



	public override void Activate() {
		renderer.enabled = false;
		collider.enabled = false;
	}

	public override void Deactivate() {
		renderer.enabled = true;
		collider.enabled = true;
	}
}
