using UnityEngine;
using System.Collections;

public class RecordedEvent {
	private Vector3 endPosition { get; set; }
	private Vector3 currentPosition { get; set; }
	public float currentTime { get; private set; }
	public float endTime { get; set; }

	public Vector3 targetPosition {
		get {
			if (GameManager.Instance.IsRewinding) {
				return currentPosition;
			}
			return endPosition;
		}
	}
	public float eventTime {
		get {
			if (GameManager.Instance.IsRewinding) {
				return endTime;
			}
			return currentTime;
		}
	}
	public float eventEndTime {
		get {
			if (GameManager.Instance.IsRewinding) {
				return currentTime;
			}
			return endTime;
		}
	}

	public bool IsTimeForEvent {
		get {
			Debug.LogError(GameManager.Instance.GameTime + " " + eventTime);
			return GameManager.Instance.IsRewinding ? GameManager.Instance.GameTime <= eventTime : GameManager.Instance.GameTime >= eventTime; 
		}
	}
	public void RunEvent(UnitModel unit) {
		unit.Move(targetPosition, this);
	}
	public RecordedEvent() { }
	public RecordedEvent(Vector3 targetPosition, Vector3 currentPosition, float currentTime) {
		this.endPosition = targetPosition;
		this.currentPosition = currentPosition;
		this.currentTime = currentTime;
	}
}