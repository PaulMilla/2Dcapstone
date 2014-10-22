using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {
	static float time;

	// Use this for initialization
	void Start () {
		//TODO: Separate this out better and/or find our previous implementation
		time = GameTime.gameTime;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public class GameTime {
		public static float gameTime;

		void Start() {
			gameTime = 0;
		}

		void Update() {
			if(Input.GetButtonDown ("Rewind")) {
				--gameTime;
			} else {
				++gameTime;
			}
		}
	}
}
