using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecordedEvent
{
	List<KeyCode> downKeys = new List<KeyCode>();

	public void AddKeyDown(KeyCode key) {
		downKeys.Add(key);
	}

	public bool GetKeyDown(KeyCode key) {
		foreach (KeyCode downKey in downKeys) {
			if (key.CompareTo(downKey) == 0){
				return true;
			}
		}
		return false;
	}
}

