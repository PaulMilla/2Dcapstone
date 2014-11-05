using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour {
	[SerializeField]
	protected float movementSpeed;
	[SerializeField]
	protected NavMeshAgent agent;
	public bool movementEnabled {get; set;}
	protected bool rewind;
    protected bool hasInteracted;
	protected RaycastHit target;
    private Interactable interactable;
    public Interactable Interactable {
        protected get { return interactable; }
        set { //BUG: Can't interact with the same object twice in a row
            if (value != interactable) {
                hasInteracted = false;
                interactable = value;
            }
        }
    }
    protected Vector3 targetPosition;
	public Stack<Event> recordedEvents;

	Animator animator;

	public void StopMovement() {
		movementEnabled = false;
		agent.Stop(true);
	}

	protected virtual void Start () {
		rewind = false;
		movementEnabled = true;
		recordedEvents = new Stack<Event>();
		targetPosition = transform.position;
		animator = GetComponent<Animator>();
	}
	protected virtual void Move() {
		Transform current = this.transform;
		animator.speed = 1f;
		animator.SetBool("Walking", true);
		if(!movementEnabled || (targetPosition - current.position).magnitude <= 0.3f) {
			agent.Stop();
			animator.SetBool("Walking", false);
		}
		if (rigidbody != null) {
			rigidbody.velocity = Vector3.zero;
		}
		/*current.rotation = Quaternion.LookRotation(targetPosition);
		current.rigidbody.velocity = Vector3.zero;
		current.rigidbody.MovePosition(Vector3.MoveTowards(current.position, targetPosition, movementSpeed * Time.fixedDeltaTime));*/
		CheckActivations();
        recordedEvents.Push(new Event(targetPosition, current.position, current.rotation, interactable, hasInteracted));
	}
	
	protected virtual Event DoRewind() {

		if (recordedEvents.Count == 0) return null;

		Event previous = recordedEvents.Pop();
		this.transform.rotation = previous.rotation;
		this.hasInteracted = previous.hasInteracted;
		Vector3 current = this.transform.position;
		Vector3 past = previous.position;
		if ((current - past).magnitude > 0) {
			animator.speed = -1f;
			animator.SetBool("Walking", true);
		} else {
			animator.SetBool("Walking", false);
		}
		Vector3 step = Vector3.MoveTowards(current, past, movementSpeed * Time.fixedDeltaTime);
		agent.Stop();

        Interactable = previous.interactable;
		this.transform.rigidbody.MovePosition(step);

		CheckActivations();
		return previous;
	}

	public virtual void MoveTo(RaycastHit hit, bool resetHasInteracted = false) {
        Interactable = hit.transform.gameObject.GetComponent<Interactable>();
		if (resetHasInteracted)
			hasInteracted = false;
		targetPosition.x = hit.point.x;
        targetPosition.z = hit.point.z;
		agent.SetDestination(hit.point);
	}

    public virtual void MoveTo(Vector3 targetPosition, Interactable interactable) {
        this.targetPosition.x = targetPosition.x;
        this.targetPosition.z = targetPosition.z;
        this.Interactable = interactable;
		agent.SetDestination(targetPosition);
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

	public NavMeshAgent GetAgent() {
		return agent;
	}
}
