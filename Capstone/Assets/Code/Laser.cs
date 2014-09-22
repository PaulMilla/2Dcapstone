using UnityEngine;
using System.Collections;

public class Laser : Activatable {

	[SerializeField]
	private LineRenderer lineRenderer;
	[SerializeField]
	private float killTime = 3;
	private GameObject currentTarget { get; set; }
	[SerializeField]
	private float maxDistance = 10000;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (!Activated) {
			return;
		}
		Ray ray = new Ray(transform.position, transform.right);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, maxDistance)) {
			lineRenderer.SetPosition(1, new Vector3(hit.distance, 0, 0));
			Debug.DrawLine(transform.position, hit.point);
			if (currentTarget != hit.collider.gameObject) {
				currentTarget = hit.collider.gameObject;
				currentTarget.SendMessageUpwards("Hit", killTime, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	public override void Activate() {
		base.Activate();
		lineRenderer.SetWidth(0.1f, 0.1f);
	}
	public override void Deactivate() {
		base.Deactivate();
		lineRenderer.SetWidth(0, 0);
	}
}
