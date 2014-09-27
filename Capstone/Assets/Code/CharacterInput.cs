using UnityEngine;
using System.Collections;

public class CharacterInput : MonoBehaviour
{
	protected PlayerMovement playerMovement;
	protected bool interactionButtonDown;

	void Awake() {
		playerMovement = GetComponent<PlayerMovement>();
	}
}

