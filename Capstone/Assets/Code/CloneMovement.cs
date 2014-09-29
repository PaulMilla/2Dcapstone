using UnityEngine;
using System.Collections;

public class CloneMovement : CharacterMovement {
	/* Inherited from CharacterMovement */
	/* overrive protected void Move() */
	/* override public void MoveTo(Vector3 pos) */

	override protected void Start () {
		base.Start();
		movementEnabled = true;
	}
	
	void FixedUpdate () {
		Move();
	}
}
