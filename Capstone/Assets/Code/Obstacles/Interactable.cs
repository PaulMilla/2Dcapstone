using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {
    public abstract void OnInteract();
    public abstract bool InPositionToInteract(CharacterMovement character);
    public abstract bool TryInteract(CharacterMovement character);
}
