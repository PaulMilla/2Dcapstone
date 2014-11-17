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
				enemy.FoundTarget(player.transform);
			}
		}
	}
}
