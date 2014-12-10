using UnityEngine;
using System.Collections;

/*
 * Attach as a child to an enemy guard
 */

public class EnemyVision : MonoBehaviour {
	Transform target;
	public static float fieldOfViewAngle = 60f;
	EnemyGuard Enemy;

	void Start () {
		Enemy = GetComponentInParent<EnemyGuard>();
		PlayerMovement.Instance.RewindEnd += () => {
			target = null;
		};
	}

	void OnTriggerStay(Collider other) {
		if (PlayerMovement.Instance.Rewind) {
			return;
		}

		// If the player is in the trigger sphere
		if (other.tag.Equals("Player") || other.tag.Equals("Hologram")) {
				
			// Create a vector from the enemy to the player
			Vector3 direction = other.transform.position - Enemy.transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			Debug.DrawRay(Enemy.transform.position,  direction.normalized * 1000, Color.white);

			if (angle < fieldOfViewAngle) {
				if (Enemy.HasLineOfSightTo(other.transform)) {
					if (target == null) {
						// New Target!!
						target = other.transform;
						Enemy.playSoundSeesPlayer();
						Enemy.FoundTarget(target);
					}
				} else if (other.transform.Equals(target)) {
					//If we don't have line of sight, cancel target
					target = null;
				}
			}
		}
	}



	// If the target leaves the trigger zone...
	void OnTriggerExit (Collider other) {
		//if(other.gameObject.transform.Equals(target)) {
		if (other.tag.Equals("Player") || other.tag.Equals("Hologram")) {
            Enemy.Investigate(other.transform.position);
			target = null;
		}
	}
}
