using UnityEngine;
using System.Collections;

public class Footstep : MonoBehaviour {
	SpriteRenderer sprite;
	public float fadeRate;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Rewind")) {
			sprite.color += new Color(0.0f, 0.0f, 0.0f, fadeRate);
			if (sprite.color.a > 1) {
				Destroy(this.gameObject);
			}
		} else {
			sprite.color -= new Color(0.0f, 0.0f, 0.0f, fadeRate);
		}
	}
}
