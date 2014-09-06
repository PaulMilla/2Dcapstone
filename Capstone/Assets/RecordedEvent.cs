using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecordedEvent
{

	// A list of keys that are down during this event
	List<KeyCode> downKeys = new List<KeyCode>();

	public void AddKey(KeyCode key) {
		downKeys.Add(key);
	}

	public bool GetKey(KeyCode key) {
		foreach (KeyCode downKey in downKeys) {
			if (key.CompareTo(downKey) == 0){
				return true;
			}
		}
		return false;
	}
}

