﻿using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void LoadScene(string SceneName) {
		Application.LoadLevel(SceneName);
	}
}
