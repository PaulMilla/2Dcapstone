using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SystemMessage : MonoBehaviour {
    private Text text;
	private Image image;
	private Image buttonImage;
	private Text buttonText;
	private Button button;
	private float oldTimeScale;
	private bool _active;
	private bool Active {
		get { return _active; }
		set {
			_active = value;
			text.enabled = value;
			image.enabled = value;
			button.enabled = value && clickToGoAway;
			buttonImage.enabled = value && clickToGoAway;
			buttonText.enabled = value && clickToGoAway;
		}
	}
	public bool clickToGoAway {get;set;}

	// Use this for initialization
	public void Start () {
        text = GetComponentInChildren<Text>();
		image = GetComponent<Image>();
		button = GetComponentInChildren<Button>();
		buttonImage = GameObject.Find("Checkmark").GetComponent<Image>();
		buttonText = GameObject.Find("Got it").GetComponent<Text>();
		oldTimeScale = Time.timeScale;
		Active = false;
	}

	public void ShowMessage(string message) {
		if(message == null) return;
		text.text = message;
		oldTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		Active = true;
	}

	public void Hide() {
		Time.timeScale = oldTimeScale;
		Active = false;
	}
}
