using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	public MovieTexture movieTexture;
	public Texture playButton;

	// Use this for initialization
	void Start () {
		movieTexture.Play ();
		movieTexture.loop = true;
	}
	
	// Update is called once per frame
	void OnGUI() {
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), movieTexture, ScaleMode.StretchToFill);
		Rect r = new Rect (Screen.width / 2 - playButton.width / 4, Screen.height / 2 + playButton.height / 3, playButton.width / 2, playButton.height / 2);
		GUI.DrawTexture (r, playButton);
	

		if (GUI.Button(r , "", new GUIStyle()))
		{
			Application.LoadLevel("Briefing");
		}

	}

}
