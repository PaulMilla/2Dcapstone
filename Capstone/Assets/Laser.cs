using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	private LineRenderer lineRenderer { get; set; }
	private GameObject currentTarget { get; set; }
	[SerializeField]
	private float maxDistance = 10000;
	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.GetComponent<LineRenderer>() as LineRenderer;
		lineRenderer.SetPosition(0, transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, maxDistance)) {
			lineRenderer.SetPosition(1, hit.point);
			if (currentTarget != hit.collider.gameObject) {
				currentTarget = hit.collider.gameObject;
				currentTarget.SendMessageUpwards("Hit", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
