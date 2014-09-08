using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {


	private bool isOpen;

	[SerializeField]
	private bool initiallyOpen = false; // By default the door is closed

	void Start() {
		if(initiallyOpen) {
			Open ();
		}
		else {
			Close();
		}
	}

	public void Reset() {
		// Since we aren't doing anything fancy in OnStart(), just call it		
		Start ();
	}

	public void Open() {
		isOpen = true;
		renderer.enabled = false;
		collider.enabled = false;
	}

	public void Close() {
		isOpen = false;
		renderer.enabled = true;
		collider.enabled = true;
	}

	public bool IsOpen() {
		return isOpen;
	}

	public void Switch() {
		if (IsOpen ()) {
			Close ();
		}
		else {
			Open ();
		}
	}
}
