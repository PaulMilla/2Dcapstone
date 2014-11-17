using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
	public static GUITextBox Dialog;
	public static SystemMessage System;

	// Use this for initialization
	void Start () {
		Dialog = GetComponentInChildren<GUITextBox>();
		System = GetComponentInChildren<SystemMessage>();
	}
}
