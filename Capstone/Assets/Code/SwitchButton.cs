using UnityEngine;
using System.Collections;

public class SwitchButton : Interactable {

	// The list of doors that we will be calling OnInteract on
	[SerializeField]
	Activatable[] activatableArray;

	void Start() {
		GameManager.Instance.RoundEnd += Reset;
	}


    public override void OnInteract()
    {
		// Just a visual indicator
		if (renderer.material.color.Equals(Color.white))
			renderer.material.color = Color.yellow;
		else
			renderer.material.color = Color.white;

		// The reason this class exists
		foreach (Activatable activatable in activatableArray) {
			activatable.Toggle();
		}
    }

    public override bool InPositionToInteract(CharacterMovement character)
    {
        return false;
    }

    public override bool TryInteract(CharacterMovement character)
    {
        if (character.collider.bounds.Intersects(this.collider.bounds))
        {
            OnInteract();
            return true;
        }
        return false;
    }

	void Reset() {

	}
}
