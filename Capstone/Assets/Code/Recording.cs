using UnityEngine;
using System.Collections.Generic;

public class Recording {
	List<RecordedEvent> eventList = new List<RecordedEvent>();
	private int currentEventIndex { get; set; }
	private int nextEventIndex { get { return GameManager.Instance.IsRewinding ? currentEventIndex - 1 : currentEventIndex + 1; } }
	public float timeNextEvent { get; private set; }
	public UnitModel unitModel { get; set; }

	public Recording(GameObject gameObject) {
		unitModel = gameObject.GetComponent<UnitModel>();
		currentEventIndex = -1;
		GameManager.Instance.RewindEvent += () => { if (GameManager.Instance.IsRewinding) currentEventIndex++; else currentEventIndex--; };
	}
	public void AddEvent(RecordedEvent recordedEvent) {
		if (GameManager.Instance.IsRewinding) {
			Debug.LogWarning("Can't record event while rewinding");
			return;
		}
		for (int i = 0; i < eventList.Count; i++) {
			if (recordedEvent.currentTime < eventList[i].currentTime) {
				eventList.RemoveAt(i--);
			}
		}
		eventList.Add(recordedEvent);
		timeNextEvent = recordedEvent.currentTime;
		Debug.LogError("AddedEvent");
	}
	public void FixedUpdate() {
		Debug.LogWarning(nextEventIndex);
		if (nextEventIndex >= 0 && eventList.Count > nextEventIndex) {
			if (eventList[nextEventIndex].IsTimeForEvent) {
				eventList[nextEventIndex].RunEvent(unitModel);
				currentEventIndex = nextEventIndex;
			}
		}
	}
}