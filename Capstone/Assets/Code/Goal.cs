using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	[SerializeField]
	Transform rightDoor;
	[SerializeField]
	Transform leftDoor;
	[SerializeField]
	float doorSpeed;

	Vector3 initialScale;

	StatTracker tracker;

	void Start() {
		initialScale = rightDoor.localScale;
		StartCoroutine(OpenDoor());
		tracker = (GameObject.FindGameObjectWithTag("UI") as GameObject).GetComponent<StatTracker>();
	}
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			StartCoroutine(CloseDoor(collider));
		}
	}
	IEnumerator OpenDoor() {
		while (rightDoor.localScale.x > 0) {
			Vector3 newScale = rightDoor.localScale;
			newScale.x -= doorSpeed * Time.deltaTime;
			newScale.x = Mathf.Max(0, newScale.x);
			rightDoor.localScale = newScale;
			leftDoor.localScale = newScale;
			yield return new WaitForEndOfFrame();
		}
		yield return null;
	}
	IEnumerator CloseDoor(Collider player) {
		while (rightDoor.localScale.x < initialScale.x) {
			Vector3 newScale = rightDoor.localScale;
			newScale.x += doorSpeed * Time.deltaTime;
			newScale.x = Mathf.Min(initialScale.x, newScale.x);
			rightDoor.localScale = newScale;
			leftDoor.localScale = newScale;
			yield return new WaitForEndOfFrame();
		}
		if (player != null) {
			Destroy(player.gameObject.GetComponent<PlayerInput>());
			PlayerMovement.Instance.StopMovement();
		}
		yield return new WaitForSeconds(1);
		tracker.ShowStats();
		yield return null;
	}
	
}
