using UnityEngine;
using System.Collections;

public class UnitModel : MonoBehaviour {
	[SerializeField]
	private NavMeshAgent navMeshAgent;

	private RecordedEvent currentEvent { get; set; }

	void Start() {
		GameManager.Instance.RewindEvent += () => { currentEvent.RunEvent(this); };
	}

	void FixedUpdate() {
		if (currentEvent == null)
			return;
		if (currentEvent.endTime == 0) {
			if (!(navMeshAgent.hasPath || navMeshAgent.pathPending)) {
				currentEvent.endTime = Time.time;
			}
		}
	}
	public void Move(Vector3 targetPosition, RecordedEvent currentEvent) {
		navMeshAgent.SetDestination(targetPosition);
		if (!GameManager.Instance.IsRewinding && this.currentEvent != null && this.currentEvent.endTime < GameManager.Instance.GameTime) {
			this.currentEvent.endTime = GameManager.Instance.GameTime;
		}
		this.currentEvent = currentEvent;
	}
}
