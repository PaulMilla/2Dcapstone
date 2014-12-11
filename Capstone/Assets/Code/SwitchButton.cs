using UnityEngine;
using System.Collections;

public class SwitchButton : Interactable {

	// The list of doors that we will be calling OnInteract on
	[SerializeField]
	Activatable[] activatableArray;

	AudioSource audioOn;
	AudioSource audioOff;

	void Start() {
		GameManager.Instance.RoundEnd += Reset;
		renderer.material.color = Color.blue;
		audioOn = transform.Find ("Audio_On").GetComponent<AudioSource> ();
		audioOff = transform.Find ("Audio_Off").GetComponent<AudioSource> ();
	}

    public override void OnInteract()
    {
		// Just a visual indicator
		if (renderer.material.color.Equals (Color.blue)) {
			audioOn.Play ();
			renderer.material.color = Color.green;
		} else {
			audioOff.Play ();
			renderer.material.color = Color.blue;
		}

		// Play some particles
		if(GetComponent<ParticleSystem>() != null)
			GetComponent<ParticleSystem>().Play();

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
