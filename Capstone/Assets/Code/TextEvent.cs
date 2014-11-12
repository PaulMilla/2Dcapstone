﻿using UnityEngine;
using System.Collections;

public class TextEvent : MonoBehaviour {
    public string text;
    public float duration;

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            UI.Dialog.ShowMessageForDuration(text, duration);
        }
    }
}
