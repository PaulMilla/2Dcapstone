using UnityEngine;
using System.Collections;

public class PlayerModel : MonoBehaviour {
	[SerializeField]
	private float movementSpeed;

	public void Move(Vector2 direction)
	{
		transform.rigidbody.MovePosition(transform.position + (Vector3)(direction * movementSpeed * Time.fixedDeltaTime));
	}
}
