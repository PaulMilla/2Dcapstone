using UnityEngine;
using System.Collections;

public class Event {
    public Vector3 targetPosition {get; private set;}
	public Vector3 position {get; private set;}
	public Quaternion rotation {get; private set;}
    public Interactable interactable {get; private set;}
	public bool hasInteracted { get; private set; }

    public Event(Vector3 tpos, Vector3 cpos, Quaternion rot, Interactable interact, bool hasInteracted)
    {
        targetPosition = cpos;
        position = cpos;
        interactable = interact;
        rotation = rot;
		this.hasInteracted = hasInteracted;
    }
}
