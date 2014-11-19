using UnityEngine;
using System.Collections;

public class StartMusic : MonoBehaviour {
	
	// Use this for initialization
	void Awake() {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void OnLevelWasLoaded (int level) {
		Debug.Log ("LEVEL: " + Application.loadedLevelName);
		if( !(Application.loadedLevelName.Equals("StartScreen") || Application.loadedLevelName.Equals("Briefing")) ) {
			Destroy(this.gameObject);
		}
	}
}
