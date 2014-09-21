using UnityEngine;
using System.Collections;

public class CharacterInput : MonoBehaviour
{

	protected PlayerModel playerModel { get; set; }
	protected bool interactionButtonDown;

	// A method which signifies whether or not this recording has pressed down the 
	// Interaction Button this frame
	public bool InteractionButtonDown() {
		return interactionButtonDown;
	}

}

