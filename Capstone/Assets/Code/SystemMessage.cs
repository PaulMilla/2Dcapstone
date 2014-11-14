using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SystemMessage : MonoBehaviour {
    private Text text;
	private Image image;
	private Image buttonImage;
	private Button button;
	private float oldTimeScale;
	private CharacterStatus playerCharacterStatus;
	private KeyCode waitUntil;
	private bool _active;
	private bool Active {
		get { return _active; }
		set {
			_active = value;
			text.enabled = value;
			image.enabled = value;
			button.enabled = value && clickToGoAway;
			buttonImage.enabled = value && clickToGoAway;
		}
	}
	public bool clickToGoAway {get;set;}

	// Use this for initialization
	public void Start () {
        text = GetComponentInChildren<Text>();
		image = GetComponent<Image>();
		button = GetComponentInChildren<Button>();
		buttonImage = GetComponentInChildren<Button>().image;
		playerCharacterStatus = FindObjectOfType<PlayerInput>().GetComponent<CharacterStatus>();
		playerCharacterStatus.Died += (isDead) => { ShowMessageUntil("You've been caught!\nPress Space to rewind.", KeyCode.Space); };
		waitUntil = KeyCode.None;
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

	public void ShowMessageUntil (string message, KeyCode keyWait) {
		if(message == null) return;
		clickToGoAway = false;
		waitUntil = keyWait;
		ShowMessage(message);
		return;
	}

	public void Hide() {
		Time.timeScale = oldTimeScale;
		Active = false;
	}

	void Update() {
		if(waitUntil != KeyCode.None && Input.GetKeyDown(waitUntil)) {
			Hide();
		}
	}
}
