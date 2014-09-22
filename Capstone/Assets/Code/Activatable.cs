using UnityEngine;
using System.Collections;

public abstract class Activatable : MonoBehaviour {

	[SerializeField]
	private bool defaultActivated = true;

	protected virtual void Start() {
		Reset();
		GameManager.Instance.RoundEnd += Reset;
	}

	public bool Activated { get; private set; }
	public virtual void Activate() {
		Activated = true;
	}
	public virtual void Deactivate() {
		Activated = false;
	}
	public void Toggle() {
		Activated = !Activated;
		if (Activated) {
			Activate();
		}
		else {
			Deactivate();
		}
	}
	public void Reset() {
		Activated = defaultActivated;
		if (Activated) {
			Activate();
		}
		else {
			Deactivate();
		}
	}
}
