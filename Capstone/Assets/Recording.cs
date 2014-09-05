using UnityEngine;
using System.Collections.Generic;

public class Recording {

	List<Vector2> movements = new List<Vector2>();
	//TODO: List<Actions (using button, or ability) and the time at which they occured>

	public void RecordMovement(Vector2 direction) {
		movements.Add(direction);
	}
}
