using UnityEngine;
using System.Collections;

public class ClockAppearRewindEffect : MonoBehaviour {

	Vector3 toScale = new Vector3 (60,60,60);
	bool animating = false;

	public float speed;

	void Start () {
		this.transform.localScale = new Vector3 (0, 0, 0);
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartAnimation ();
		}
		else if (Input.GetKeyUp(KeyCode.Space)) {
			Reset ();
		}

		if (animating) {
			Animate ();
		}
	}

	void StartAnimation() {
		animating = true;
	}

	void Reset() {
		animating = false;
		this.transform.localScale = Vector3.zero;
	}

	void Animate() {
		if (this.transform.localScale.magnitude < toScale.magnitude) {
			this.transform.localScale = Vector3.MoveTowards (this.transform.localScale, toScale, speed);
		}
	}
}
