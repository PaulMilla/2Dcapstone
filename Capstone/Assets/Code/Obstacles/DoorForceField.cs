using UnityEngine;
using System.Collections;

public class DoorForceField : Door
{	
	private int imagesIndex = 1;
	public Texture[] images;
	private int direction = 1;
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

