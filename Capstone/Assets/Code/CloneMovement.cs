using UnityEngine;
using System.Collections;

public class CloneMovement : CharacterMovement {
	override protected void Start () {
		base.Start();
		movementEnabled = true;
	}
	
	void FixedUpdate () {
		Move();
	}
}
