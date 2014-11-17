using UnityEngine;
using System.Collections;

public class StopTime : MonoBehaviour {

	public bool stopTime =false;

	Animator anim;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (stopTime) {
			Time.timeScale = 0.0f;
		}

	
	}
}
