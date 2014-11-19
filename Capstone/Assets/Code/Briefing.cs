using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Briefing : MonoBehaviour {
	GameObject briefing1;
	GameObject briefing2;
	GameObject button1;
	GameObject button2;
	GameObject playButton;
	public string nextScene = "Tutorial";

	public AudioSource AudioClick;
	public AudioSource AudioMouseOver;

	// Use this for initialization
	void Start () {
		briefing1 = GameObject.Find("Briefing1");
		briefing2 = GameObject.Find("Briefing2");
		button1 = GameObject.Find("Button1");
		button2 = GameObject.Find("Button2");
		playButton = GameObject.Find("PlayButton");
		GoToPage1();
	}

	public void GoToPage1() {
		AudioClick.Play ();
		briefing2.SetActive(false);
		button1.SetActive(false);
		button2.SetActive(true);
		briefing1.SetActive(true);
		playButton.SetActive(false);
	}

	public void GoToPage2() {
		AudioClick.Play ();
		briefing2.SetActive(true);
		button1.SetActive(true);
		button2.SetActive(false);
		briefing1.SetActive(false);
		playButton.SetActive(true);
	}

	public void PlayGame() {
		AudioClick.Play ();
		Application.LoadLevel(nextScene);
	}
}
