using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecordedEvent
{

	// A list of keys that are down during this event
	List<KeyCode> keys = new List<KeyCode>();

	// A list of keys that went down this event
	List<KeyCode> downedKeys = new List<KeyCode>();

	public void AddKey(KeyCode key) {
		keys.Add(key);
	}

	public void AddKeyDown(KeyCode key) {
		downedKeys.Add(key);
	}

	public bool GetKey(KeyCode key) {
		foreach (KeyCode downKey in keys) {
			if (key.CompareTo(downKey) == 0){
				return true;
			}
		}
		return false;
	}

	public bool GetKeyDown(KeyCode key) {
		foreach (KeyCode downedKey in downedKeys) {
			if (key.CompareTo(downedKey) == 0){
				return true;
			}
		}
		return false;
	}
}

