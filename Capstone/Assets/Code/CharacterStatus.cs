using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {
	protected CharacterMovement characterMovement;
	private bool _isDead;
	public bool isDead { 
		get { return _isDead; } 
		private set { 
			if (Died != null) { 
				Died(value); 
			} 
			_isDead = value; } 
	}

	public bool dissolve = false;
	Animator animator;
	private Vector3 deathPosition;

	public delegate void BoolHandler(bool b); 
	public BoolHandler Died;

	SkinnedMeshRenderer meshRenderer;
	// Use this for initialization
	void Start() {
		characterMovement = GetComponent<CharacterMovement>();
		animator = GetComponent<Animator>();
		meshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer> ();
	}

	void Update() {
		if (isDead) {
			transform.position = deathPosition;
		}
		if (Input.GetButtonDown("Rewind")) {
			if (isDead) {
				Time.timeScale = 1.0f;
				characterMovement.movementEnabled = true;
				isDead = false;
			}
		}

		if (dissolve) {						
			Dissolve ();				
		}
	}

	float dissolveParam = 0.0f;
	public float dissolveSpeed;
	Transform matTransform;

	void Dissolve() {
		dissolveParam += dissolveSpeed;
		meshRenderer.renderer.material.SetFloat ("_SliceAmount", dissolveParam);
	}

	public void Hit(float killTime) {
		if (Input.GetButton("Rewind")) {
			return;
		}
		characterMovement.StopMovement();
		deathPosition = transform.position;
		isDead = true;
		if (characterMovement is PlayerMovement) {
				Time.timeScale = 0f;
				return;
		} else {
			Die(2);
		}
		StartCoroutine(Die(killTime));
	}

	IEnumerator Die(float killTime) {
		dissolve = true;
		animator.SetBool("Dead", true);
		yield return new WaitForSeconds(killTime);
		GetKilled();
		yield return null;
	}

	public void GetKilled() {
		if (isDead) 
			Destroy(this.gameObject);
	}
}
