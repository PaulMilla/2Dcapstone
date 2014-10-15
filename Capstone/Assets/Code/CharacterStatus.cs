using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {
	protected CharacterMovement characterMovement;
	public bool isDead { get; private set; }

	// Use this for initialization
	void Start() {
		characterMovement = GetComponent<CharacterMovement>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			if (isDead) {
				characterMovement.movementEnabled = true;
				isDead = false;
			}
		}
	}
	public void Hit(float killTime) {
		if (Input.GetKey(KeyCode.R)) {
			return;
		}
		characterMovement.StopMovement();
		isDead = true;
		if (characterMovement as PlayerMovement != null) {
			//Time.timeScale = 0;
			return;
		}
		StartCoroutine(Die(killTime));
	}

	IEnumerator Die(float killTime) {
		yield return new WaitForSeconds(killTime);
		GetKilled();
		yield return null;
	}

	public void GetKilled() {
		if (isDead) 
			Destroy(this.gameObject);
	}
}
