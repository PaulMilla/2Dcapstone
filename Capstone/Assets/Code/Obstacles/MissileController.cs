using UnityEngine;
using System.Collections;

public class MissileController : Activatable {
	
	Transform target; 
	Vector3 lastDetectedPosition;
	Vector3 startingPosition;
	bool isInMotion = false;
	
	public float Speed;

	protected override void Start() {
		base.Start();
		startingPosition = this.transform.position;
	}

	void FixedUpdate() {
		if (isInMotion) {
			Debug.Log ("InMotion");
			if (target != null) {
				this.rigidbody.MovePosition (Vector3.MoveTowards (this.transform.position, target.position, Time.fixedDeltaTime * Speed));
			} else {
				this.rigidbody.MovePosition (Vector3.MoveTowards (this.transform.position, lastDetectedPosition, Time.fixedDeltaTime * Speed));
			}
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (other.transform.tag.Equals ("Player") ||
		    other.transform.tag.Equals ("Hologram")) {
			Debug.Log("Player or Hologram in");
			// Do we have a LOS to the target?

			Vector3 direction = other.transform.position - this.transform.position;
			Debug.DrawRay(this.transform.position,  direction.normalized * 1000,Color.white);

			RaycastHit hit;
			if (Physics.Raycast(this.transform.position, other.transform.position - this.transform.position, out hit, 1000)) {
				
				if (hit.collider.transform.Equals(other.transform)) {
					Debug.Log("Hit other");
					// We have LOS to "other", a player or a hologram
					isInMotion = true;
					// If we already have a target but this new one is closer
					if (target != null && (target.transform.position - this.transform.position).magnitude >
										  (other.transform.position - this.transform.position).magnitude) {
							target = other.transform;
					}
					else if (target == null) {
						target = other.transform;
					}
				}
			} else {
				// We didn't hit the collider, so check if it was our target. 
				// If so, this means we lost track of the target
				if (target != null && !other.transform.Equals(target)) {
					target = null;
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.transform.Equals (target)) {
			lastDetectedPosition = target.position;
			target = null;
		}
	}

	public override void Reset(){ 
		this.transform.position = startingPosition;
		isInMotion = false;
		target = null;
	}
}
