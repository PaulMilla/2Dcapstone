using UnityEngine;
using System.Collections;

public class DelayedDestruction : MonoBehaviour {

	public float lifeTime = 3.0f;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, lifeTime);
	}
}
