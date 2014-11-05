using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathText : MonoBehaviour {
	CharacterStatus playerCharacterStatus;

	void Start() {
		playerCharacterStatus = FindObjectOfType<PlayerInput>().GetComponent<CharacterStatus>();
		playerCharacterStatus.Died += (isDead) => { gameObject.SetActive(isDead); };
		GetComponentInChildren<Text>().enabled = true;
		GetComponent<Image>().enabled = true;
		gameObject.SetActive(false);
	}
}
