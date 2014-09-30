﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour {
	[SerializeField]
	protected float movementSpeed;
	public bool movementEnabled {get; set;}
	protected bool rewind;
    protected bool hasInteracted;
	protected RaycastHit target;
    private Interactable interactable;
    public Interactable Interactable {
        protected get { return interactable; }
        set {
            if (value != interactable) {
                hasInteracted = false;
                interactable = value;
            }
        }
    }
    protected Vector3 targetPosition;
	public Stack<Event> recordedEvents;

	protected virtual void Start () {
		rewind = false;
		movementEnabled = false;
		recordedEvents = new Stack<Event>();
	}

	protected virtual void Move() {
		Transform current = this.transform;
		if(!movementEnabled || (targetPosition - current.position).magnitude <= 0.1f)
			return;
		current.rotation = Quaternion.LookRotation(targetPosition);
		current.rigidbody.velocity = Vector3.zero;
		current.rigidbody.MovePosition(Vector3.MoveTowards(current.position, targetPosition, movementSpeed * Time.fixedDeltaTime));
		CheckActivations();
        recordedEvents.Push(new Event(targetPosition, current.position, current.rotation, interactable));
	}
	
	protected virtual Event DoRewind() {
		if (recordedEvents.Count == 0) return null;

		Event previous = recordedEvents.Pop();
		this.transform.rotation = previous.rotation;
		Vector3 current = this.transform.position;
		Vector3 past = previous.position;
		Vector3 step = Vector3.MoveTowards(current, past, movementSpeed * Time.fixedDeltaTime);
        Interactable = previous.interactable;
		this.transform.rigidbody.MovePosition(step);

		CheckActivations();
		return previous;
	}

	public virtual void MoveTo(RaycastHit hit) {
        interactable = hit.transform.gameObject.GetComponent<Interactable>();
		targetPosition.x = hit.point.x;
        targetPosition.z = hit.point.z;
	}

    public virtual void MoveTo(Vector3 targetPosition, Interactable interactable) {
        this.targetPosition.x = targetPosition.x;
        this.targetPosition.z = targetPosition.z;
        this.Interactable = interactable;
    }
	
	public virtual void CheckActivations() {
        if (interactable != null && !hasInteracted && interactable.TryInteract(this)) {
            hasInteracted = true;
        }
	}

	public virtual void ClearTarget() {
        targetPosition = this.transform.position;
        hasInteracted = true;
	}
}
