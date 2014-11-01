using UnityEngine;
using System.Collections;

public class EnergyDoor : Door
{	
	private int imagesIndex = 1;
	public Texture[] images;
	private int direction = 1;

	AudioSource audioActivate;
	AudioSource audioDeactivate;

	void Start() {
		audioActivate = transform.Find ("AudioActivate").gameObject.GetComponent<AudioSource> ();
		audioDeactivate = transform.Find ("AudioDeactivate").gameObject.GetComponent<AudioSource> ();
	}

	protected override void Activate() {
		renderer.enabled = false;
		collider.enabled = false;
		audioActivate.Play ();
	}
	
	protected override void Deactivate() {
		renderer.enabled = true;
		collider.enabled = true;
		audioDeactivate.Play ();
	}

	void LateUpdate() 
	{
		if (renderer.enabled) {
			imagesIndex += direction;
			if (imagesIndex >= images.Length-1 || imagesIndex <= 0) {
				direction = -direction;
			}  	 
			renderer.material.mainTexture = images[imagesIndex];
		} 
	}
}

