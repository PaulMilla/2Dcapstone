using UnityEngine;
using System.Collections;

public class EnemyVision : MonoBehaviour {

	Transform target;

	public EnemyGuard Enemy;
	
	void OnTriggerStay(Collider other) {
		// If the player is in the trigger sphere
		if (other.tag.Equals("Player") ||
		   other.tag.Equals("Hologram")) {
				
			Debug.Log("Player Entered");
			// Create a vector from the enemy to the player
			Vector3 direction = other.transform.position - Enemy.transform.position;

			Debug.DrawLine(Enemy.transform.position, Enemy.transform.position + Enemy.transform.right * 10, Color.blue);
			Debug.DrawLine(Enemy.transform.position, Enemy.transform.position + Enemy.transform.up * 10, Color.red);
			Debug.DrawLine(Enemy.transform.position, Enemy.transform.position + Enemy.transform.forward * 10, Color.yellow);

			Debug.DrawRay(Enemy.transform.position,  direction.normalized * 1000,Color.white);


			if (Enemy.HasLineOfSightTo(other.transform)) {
				if (target != null) {
					// Already have a target, switch if new target is closer
					if (direction.magnitude < (other.transform.position - Enemy.transform.position).magnitude) {
						Debug.Log("Target Set");
						target = other.transform;
					}
				}
				else {
					Debug.Log("Target Set");
					target = other.transform;
				}
			}
		
		}
	}

	void OnTriggerExit (Collider other)
	{
		// If the target leaves the trigger zone...
		if(other.gameObject.transform == target) {
			target = null;
		}
	}

	public bool HasTarget() {
		return target != null;
	}

	public Transform GetTarget() {
		return target;
	}
}
