using UnityEngine;
using System.Collections;

public class CloneMovement : CharacterMovement {
	public bool Rewind {
		get { return rewind; }
		set { rewind = value;}
	}

	/* Inherited from CharacterMovement */
	/* overrive protected void Move() */
	/* override public void MoveTo(Vector3 pos) */

	override protected void Start () {
		base.Start();
		movementEnabled = true;
	}
	
	void FixedUpdate () {
		if(rewind) {
			GameObject.Destroy(gameObject);
		} else {
			Move();
		}
	}
}
