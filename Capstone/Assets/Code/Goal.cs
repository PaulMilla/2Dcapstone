using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			GameManager.Instance.LoadNextLevel();
		}
	}
}
