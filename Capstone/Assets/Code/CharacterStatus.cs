using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {
	protected CharacterMovement characterMovement;

	// Use this for initialization
	void Start() {
		characterMovement = GetComponent<CharacterMovement>();
	}

	public void Hit(float killTime) {
		characterMovement.movementEnabled = false;
		StartCoroutine(Die(killTime));
	}

	IEnumerator Die(float killTime) {
		yield return new WaitForSeconds(killTime);
		GetKilled();
		yield return null;
	}

	public void GetKilled() {
		Destroy(this.gameObject);
	}
}
