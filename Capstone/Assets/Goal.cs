using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	void OnTriggerEnter(Collider collider) {
		GameManager.Instance.LoadNextLevel();
	}
}
