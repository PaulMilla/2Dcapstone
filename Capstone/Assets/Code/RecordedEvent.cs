using UnityEngine;
using System.Collections;

public class RecordedEvent {
	public Vector3 endPosition { get;  set; }
	public Vector3 startPosition { get; private set; }
	public float startTime { get; private set; }
	public float endTime { get; set; }
	public EventType eventType { get; private set; }

	public Vector3 targetPosition {
		get {
			if (GameManager.Instance.IsRewinding) {
				return startPosition;
			}
			return endPosition;
		}
	}
	public float eventTime {
		get {
			if (GameManager.Instance.IsRewinding) {
				return endTime;
			}
			return startTime;
		}
	}
	public float eventEndTime {
		get {
			if (GameManager.Instance.IsRewinding) {
				return startTime;
			}
			return endTime;
		}
	}

	public bool IsTimeForEvent {
		get {
			return GameManager.Instance.IsRewinding ? GameManager.Instance.GameTime <= eventTime : GameManager.Instance.GameTime >= eventTime; 
		}
	}
	public void RunEvent(UnitModel unit) {
		Debug.Log(eventType);
		if (eventType == EventType.Move)
			unit.Move(this);
		else if (eventType == EventType.Create) {
			if (GameManager.Instance.IsRewinding) {
				unit.gameObject.SetActive(false);
			}
			else {
				unit.gameObject.SetActive(true);
			}
		}
	}
	public RecordedEvent() { }
	public RecordedEvent(Vector3 targetPosition, Vector3 currentPosition, float currentTime, EventType eventType) {
		this.endPosition = targetPosition;
		this.startPosition = currentPosition;
		this.startTime = currentTime;
		this.endTime = float.MaxValue;
		this.eventType = eventType;
	}
	public RecordedEvent(Vector3 targetPosition, Vector3 currentPosition, float currentTime, float endTime, EventType eventType) {
		this.endPosition = targetPosition;
		this.startPosition = currentPosition;
		this.startTime = currentTime;
		this.endTime = endTime;
		this.eventType = eventType;
	}

	public override string ToString() {
		return "TYPE: " + eventType + " startPosition: " + startPosition + " startTime: " + startTime + "EndPosition: " + endPosition + " EndTime: " + endTime;
	}
}

public enum EventType {
	Move,
	Create,
	Destroy
};