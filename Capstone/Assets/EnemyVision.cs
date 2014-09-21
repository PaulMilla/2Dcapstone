﻿using UnityEngine;
using System.Collections;

/*
 * Attach as a child to an enemy guard
 */

public class EnemyVision : MonoBehaviour {

	Transform target;

	float fieldOfViewAngle = 55f;

	EnemyGuard Enemy;

	void Start () {
		Enemy = GetComponentInParent<EnemyGuard>();
	}

	void OnTriggerStay(Collider other) {
		// If the player is in the trigger sphere
		if (other.tag.Equals("Player") || other.tag.Equals("Hologram")) {
				
			// Create a vector from the enemy to the player
			Vector3 direction = other.transform.position - Enemy.transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			Debug.DrawRay(Enemy.transform.position,  direction.normalized * 1000,Color.white);

			if (angle < fieldOfViewAngle) {
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
	}

	void OnTriggerExit (Collider other)
	{
		// If the target leaves the trigger zone...
		if(other.gameObject.transform.Equals(target)) {
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
