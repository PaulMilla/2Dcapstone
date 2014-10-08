using UnityEngine;
using System.Collections;

public class UnitModel : MonoBehaviour {
	[SerializeField]
	private NavMeshAgent navMeshAgent;

	private RecordedEvent currentEvent { get; set; }

	void Start() {
		//GameManager.Instance.RewindEvent += () => { if (GameManager.Instance.IsRewinding && currentEvent != null) currentEvent.RunEvent(this); };
	}

	void FixedUpdate() {
		if (currentEvent == null)
			return;
		if (currentEvent.endTime == float.MaxValue) {
			if (!(navMeshAgent.hasPath || navMeshAgent.pathPending)) {
				currentEvent.endTime = GameManager.Instance.GameTime;
			}
		}
	}
	public void Move(RecordedEvent currentEvent) {
		navMeshAgent.SetDestination(currentEvent.targetPosition);
		if (!GameManager.Instance.IsRewinding && this.currentEvent != null && this.currentEvent.endTime == float.MaxValue) {
			this.currentEvent.endTime = GameManager.Instance.GameTime;
			this.currentEvent.endPosition = transform.position;
		}
		this.currentEvent = currentEvent;
	}
}
