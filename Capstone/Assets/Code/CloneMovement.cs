using UnityEngine;
using System.Collections;

public class CloneMovement : CharacterMovement {

	[SerializeField]
	private GameObject deathParticle;
	private bool dead;

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
		if (GameState.Paused) {
			return;
		}
		if (dead) {
			return;
		}
		if(rewind && !dead) {
			StartCoroutine(DeathCoroutine());
			dead = true;
			agent.Stop();
		} else {
			Move();
		}
	}

	IEnumerator DeathCoroutine() {
		GameObject.Instantiate(deathParticle, transform.position, transform.rotation);
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}

	void PlayFootstepSound() {
		// Play hologram footstep
	}
}
