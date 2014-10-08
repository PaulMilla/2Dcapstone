using UnityEngine;
using System.Collections.Generic;

public class Recording {
	List<RecordedEvent> eventList = new List<RecordedEvent>();
	private int currentEventIndex { get; set; }
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
		/*for (int i = 0; i < eventList.Count; i++) {
			if (recordedEvent.startTime < eventList[i].startTime) {
				eventList.RemoveAt(i--);
			}
		}*/
		eventList.Add(recordedEvent);
		timeNextEvent = recordedEvent.startTime;
	}
	public void FixedUpdate() {
		
		if (!GameManager.Instance.IsRewinding) {
			if (currentEventIndex < eventList.Count - 1) {
				if (eventList[currentEventIndex + 1].IsTimeForEvent) {
					eventList[currentEventIndex + 1].RunEvent(unitModel);
					//Debug.Log(eventList[currentEventIndex + 1]);
					currentEventIndex++;
				}
			}
		}
		else {
			if (unitModel.name == "HologramCharacter(Clone)") {
				Debug.Log(currentEventIndex + " " + eventList.Count + " " + eventList[0]);
			}
			if (currentEventIndex > 0) {
				if (eventList[currentEventIndex - 1].IsTimeForEvent) {
					eventList[currentEventIndex - 1].RunEvent(unitModel);
					//Debug.Log(eventList[currentEventIndex - 1]);
					currentEventIndex--;
				}
			}
		}
	}

	public void Split(UnitModel unitModel) {
		unitModel = GameObject.Instantiate(unitModel, this.unitModel.transform.position, this.unitModel.transform.rotation) as UnitModel;
		Recording newRecording = new Recording(unitModel.gameObject);
		GameManager.Instance.AddRecording(newRecording);
		if (currentEventIndex >= 0) {
			RecordedEvent currentEvent = eventList[currentEventIndex];
			newRecording.AddEvent(new RecordedEvent(unitModel.transform.position, unitModel.transform.position, GameManager.Instance.GameTime, GameManager.Instance.GameTime, EventType.Create));
			newRecording.AddEvent(new RecordedEvent(currentEvent.endPosition, unitModel.transform.position, GameManager.Instance.GameTime, currentEvent.endTime, currentEvent.eventType));
			eventList[currentEventIndex] = new RecordedEvent(unitModel.transform.position, currentEvent.startPosition, currentEvent.startTime, GameManager.Instance.GameTime, currentEvent.eventType);
			eventList[currentEventIndex].RunEvent(unitModel);
		}
		for(int i = currentEventIndex + 1; i < eventList.Count; i++) {
			newRecording.AddEvent(eventList[i]);
			eventList.RemoveAt(i--);
		}
	}
}