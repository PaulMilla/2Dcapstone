using UnityEngine;
using System.Collections;

public class AlertGuardsScript : Activatable {

	[SerializeField]
	private EnemyGuard[] enemies;
	[SerializeField]
	private PlayerMovement player;


	bool beenActivated;

	protected override void Activate () {
		if (!beenActivated) {
			beenActivated = true;
			foreach (EnemyGuard enemy in enemies) {
				enemy.SetDestination(player.transform.position, enemy.PursuitSpeed);
				enemy.movementEnabled = true;
				enemy.enabled = true;
				enemy.SetTarget(player.transform);
			//	enemy.transform.LookAt(player.transform.position);
			}
		}
	}
}
