using UnityEngine;
using System.Collections;

public class Payload : MonoBehaviour {

	MissileController missileController;
	public float blastRadius;

	void Start () {
		missileController = GetComponentInParent<MissileController> ();
	}

	void OnTriggerEnter(Collider other) {
		// Blow Up!
		Debug.Log ("Explosion!");
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
