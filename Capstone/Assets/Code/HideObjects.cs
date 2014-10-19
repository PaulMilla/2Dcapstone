using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HideObjects : MonoBehaviour {

	private Transform prevTransObject;
	public float alphaValue = 0.5f; // our alpha value
	public List<string> transparentTags = new List<string>();   // transparency layers.
	public Transform target;
	public float sphereCastRadius;

	List<Transform> previousObjects = new List<Transform>();
	RaycastHit[] hits;
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		// Cast ray from camera.position to target.position and check if the specified layers are between them.
		Ray ray = new Ray(this.transform.position, (target.position - this.transform.position).normalized);
		hits = Physics.SphereCastAll (ray, sphereCastRadius, (target.transform.position - this.transform.position).magnitude, 1 << LayerMask.NameToLayer("Hideable"));
		foreach (Transform previousObject in previousObjects) {
			if (!inHits(previousObject)) {
				setAlpha(previousObject, 1.0f);
			}
		}

		previousObjects.Clear ();
		foreach (RaycastHit hit in hits) {
			setAlpha(hit.transform, alphaValue);
			previousObjects.Add(hit.transform);
		}



//		RaycastHit transHit;
//		if (Physics.Raycast(ray, out transHit, (target.transform.position - this.transform.position).magnitude))
//		{
//			Transform objectHit = transHit.transform;
//			if(transparentTags.Contains(objectHit.gameObject.tag))
//			{
//				if(prevTransObject != null) {
//					setAlpha(prevTransObject, 1.0f);
//				}
//				if(objectHit.renderer != null) {
//					prevTransObject = objectHit;
//					setAlpha(prevTransObject, alphaValue);
//				}
//			}
//			else if(prevTransObject != null)
//			{
//				setAlpha(prevTransObject, 1.0f);
//				prevTransObject = null;
//			}
//		}
	}

	bool inHits(Transform t) {
		foreach (RaycastHit hit in hits) {
			if (t.Equals(hit.transform)) {
				return true;
			}
		}
		return false;
	}

	void setAlpha(Transform obj, float alpha) {
		Color color = obj.renderer.material.color;
		color.a = alpha;
		obj.renderer.material.color = color;
	}
}
