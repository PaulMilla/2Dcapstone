using UnityEngine;
using System.Collections;

public class PlayerModel : MonoBehaviour {
	[SerializeField]
	private float movementSpeed;
	[SerializeField]
	private float deathTime;
	
	private Vector3 positionToMoveTo;
	
	private bool movementEnabled = false;
	
	void FixedUpdate() {
		if (movementEnabled)
			Move ();
	}
	
	private void Move()
	{
		if ((positionToMoveTo - this.transform.position).magnitude > 0.1f) {
			this.transform.rigidbody.velocity = Vector3.zero;
			this.transform.rigidbody.rotation.SetLookRotation(positionToMoveTo);
			this.transform.rigidbody.MovePosition(Vector3.MoveTowards(this.transform.position, positionToMoveTo, movementSpeed * Time.fixedDeltaTime));
		}
	}
	
	public void MoveTo(Vector3 pos) {
		movementEnabled = true;
		positionToMoveTo = pos;
		positionToMoveTo.y = this.transform.position.y;
	}
	public void Hit() {
		Debug.Log("Hit");
		movementSpeed = 0;
		positionToMoveTo = transform.position;
		StartCoroutine(Die());
	}
	IEnumerator Die() {
		yield return new WaitForSeconds(deathTime);
		GetKilled();
		yield return null;
	}
	public void GetKilled() {
		Destroy(this.gameObject);
	}
}