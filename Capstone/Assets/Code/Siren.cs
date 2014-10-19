using UnityEngine;
using System.Collections;

public class Siren : Activatable {
	
	[SerializeField]
	private float sustainTime = 2.0f;
	private float timer;

	void Start() {
		timer = sustainTime;
	}

	void Update () {
		if (this.Activated) {
			timer += Time.deltaTime;
			if (timer >= sustainTime) {
				this.light.enabled = !this.light.enabled;
				timer = 0.0f;
			}
		}
	}
}
