using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : CharacterMovement {
	
	public Stack<Event> cloneEvents {get; private set;}

	public static PlayerMovement Instance;
	[SerializeField]
	private float rewindRadius = 8;
	public bool Rewind {
		get { return rewind; }
		set {
			if (rewind == true && value == false && !OutOfGuardVision()) {
				StartCoroutine(RewindUntilOutOfVision());
				return;
			}
			hasInteracted = false;
			rewind = value;
			if (rewind) {
				if (RewindBegin != null) {
					RewindBegin();
				}
				cloneEvents = new Stack<Event>();
			} else {
				if (RewindEnd != null) {
					RewindEnd();
				}
				ClearTarget();
			}
		}
	}

	public delegate void PlayerEvent();
	public PlayerEvent RewindEnd;
	public PlayerEvent RewindBegin;
	/* Inherited from CharacterMovement */
	/* overrive protected void Move() */
	/* override public void MoveTo(RaycastHit target) */
	void Awake() {
		Instance = this;
	}

	override protected void Start() {
		base.Start();
	}

	void FixedUpdate() {
		if (GameState.Paused) {
			return;
		}
		if(Rewind) {
			cloneEvents.Push(DoRewind());
		} else {
			Move();
		}
	}
	IEnumerator RewindUntilOutOfVision() {
		while (true) {
			if (OutOfGuardVision()) {
				Rewind = false;
				break;
			}
			yield return new WaitForEndOfFrame();
		}
	}
	public bool OutOfGuardVision() {
		if (!Rewind) {
			return true;
		}
		Collider[] nearyByEnemies = Physics.OverlapSphere(transform.position, rewindRadius, 1 << LayerMask.NameToLayer("Enemy"));
		bool outOfGuardVision = true;
		foreach (Collider enemyCollider in nearyByEnemies) {
			if(enemyCollider.gameObject.GetComponent<EnemyGuard>().InVisionCone(this.transform)) {
				return false;
			} 
		}
		return outOfGuardVision;
	}
}