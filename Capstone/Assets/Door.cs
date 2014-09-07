using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	

	public void Open() {
		renderer.enabled = false;
		collider.enabled = false;
	}

	public void Close() {
		renderer.enabled = true;
		collider.enabled = true;
	}
}
