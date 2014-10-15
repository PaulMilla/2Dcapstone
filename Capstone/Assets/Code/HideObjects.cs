using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HideObjects : MonoBehaviour {

	private Transform prevTransObject;
	public float alphaValue = 0.5f; // our alpha value
	public List<string> transparentTags = new List<string>();   // transparency layers.
	public Transform target;


	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		// Cast ray from camera.position to target.position and check if the specified layers are between them.
		Ray ray = new Ray(this.transform.position, (target.position - this.transform.position).normalized);
		RaycastHit transHit;
		if (Physics.Raycast(ray, out transHit, (target.transform.position - this.transform.position).magnitude))
		{
			Transform objectHit = transHit.transform;
			if(transparentTags.Contains(objectHit.gameObject.tag))
			{
				if(prevTransObject != null)
				{
					prevTransObject.renderer.material.color = new Color(1,1,1,1);
				}
				
				if(objectHit.renderer != null)
				{
					prevTransObject = objectHit;
					
					// Can only apply alpha if this material shader is transparent.
					prevTransObject.renderer.material.color = new Color(1,1,1, alphaValue);
				}
			}
			else if(prevTransObject != null)
			{
				prevTransObject.renderer.material.color = new Color(1,1,1,1);
				prevTransObject = null;
			}
		}
	}
}
