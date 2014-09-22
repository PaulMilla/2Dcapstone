using UnityEngine;
using System.Collections;

public class Laser : Activatable {

	[SerializeField]
	private LineRenderer lineRenderer;
	private GameObject currentTarget { get; set; }
	[SerializeField]
	private float maxDistance = 10000;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (!Activated) {
			lineRenderer.SetWidth(0, 0);
			return;
		}
		Debug.Log(transform.right);
		Ray ray = new Ray(transform.position, transform.right);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, maxDistance)) {
			lineRenderer.SetPosition(1, new Vector3(hit.distance, 0, 0));
			Debug.DrawLine(transform.position, hit.point);
			if (currentTarget != hit.collider.gameObject) {
				currentTarget = hit.collider.gameObject;
				currentTarget.SendMessageUpwards("Hit", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
