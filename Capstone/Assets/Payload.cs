using UnityEngine;
using System.Collections;

public class Payload : MonoBehaviour {

	MissileController missileController;

	private static float DEFAULT_BLAST_RADIUS = 5.0f;
	public float blastRadius;

	void Start () {
		missileController = GetComponentInParent<MissileController> ();
		if (blastRadius <= 0) {
			blastRadius = DEFAULT_BLAST_RADIUS;
		}
	}

	void OnTriggerEnter(Collider other) {
 		// Blow Up!
		// Check surrounding area for victims
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, blastRadius);
		int i = 0;
		while (i < hitColliders.Length) {
			if (hitColliders[i].tag.Equals("Player") ||
			    hitColliders[i].tag.Equals("Hologram") ||
			    hitColliders[i].tag.Equals("EnemyGuard")) 
			{
				PlayerModel playerModel = hitColliders[i].gameObject.GetComponent<PlayerModel>();
				playerModel.GetKilled();
			}
			i++;
		}

		// Instantiate an Explosion Prefab and go back to launch position
		missileController.Reset ();
	}
}
