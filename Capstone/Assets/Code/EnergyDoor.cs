using UnityEngine;
using System.Collections;

public class EnergyDoor : Door
{	
	private int imagesIndex = 1;
	public Texture[] images;
	private int direction = 1;

	private float dissolveAmount = 0.0f;
	private const float dissolveSpeed = 0.04f;

	AudioSource audioActivate;
	AudioSource audioDeactivate;
	
	void Awake() {
		audioActivate = transform.Find ("AudioActivate").gameObject.GetComponent<AudioSource> ();
		audioDeactivate = transform.Find ("AudioDeactivate").gameObject.GetComponent<AudioSource> ();
	}

	void Update() {
		if (this.Activated) {
			dissolve ();
		} else {
			appear();
		}
	}




	protected override void Activate() {
		//renderer.enabled = false;
		collider.enabled = false;
		audioActivate.Play ();
	}
	
	protected override void Deactivate() {
		//renderer.enabled = true;
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

	private void dissolve() {
		dissolveAmount += dissolveSpeed;
		dissolveAmount = Mathf.Clamp(dissolveAmount, 0.0f, 1.0f);
		this.renderer.material.SetFloat ("_SliceAmount", dissolveAmount);
	}

	private void appear() {
		dissolveAmount -= dissolveSpeed;
		dissolveAmount = Mathf.Clamp(dissolveAmount, 0.0f, 1.0f);
		this.renderer.material.SetFloat ("_SliceAmount", dissolveAmount);
	}
}

