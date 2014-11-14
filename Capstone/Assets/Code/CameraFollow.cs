using UnityEngine;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;
	private bool panning;

	[SerializeField]
	List<CameraPanEvent> PanEvents; 

    Vector3 offset;

    void Start() {
        offset = transform.position - target.position;
    }

    void FixedUpdate() {
		Debug.LogError("Trnsfrom " + transform.position);
		if (panning)
			return;
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
		for (int i = 0; i < PanEvents.Count; i++) {
			if (target.collider.bounds.Intersects(PanEvents[i].triggerCollider.bounds)) {
				StartCoroutine(PanCamera(PanEvents[i]));
				PanEvents.RemoveAt(i--);
				break;
			}
		}
    }

	System.Collections.IEnumerator PanCamera(CameraPanEvent panEvent) {
		panning = true;
		panEvent.cameraPosition += transform.parent.position;
		Vector3 initialPosition = transform.position;
		float initialSize = Camera.main.orthographicSize;
		float distance = 0;
		while (transform.position != panEvent.cameraPosition) {
			transform.position = Vector3.Lerp(initialPosition, panEvent.cameraPosition, Mathf.Min(1, distance + panEvent.panSpeed * Time.deltaTime));
			Debug.Log(panEvent.cameraPosition + " " + transform.position);
			Camera.main.orthographicSize = Mathf.Lerp(initialSize, panEvent.zoomCameraSize, Mathf.Min(1, distance + panEvent.panSpeed * Time.deltaTime));
			distance += panEvent.panSpeed * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(panEvent.lookDuration);
		distance = 0;
		while (transform.position != initialPosition) {
			transform.position = Vector3.Lerp(panEvent.cameraPosition, initialPosition, Mathf.Min(1, distance + panEvent.panSpeed * Time.deltaTime));
			Camera.main.orthographicSize = Mathf.Lerp(panEvent.zoomCameraSize, initialSize, Mathf.Min(1, distance + panEvent.panSpeed * Time.deltaTime));
			distance += panEvent.panSpeed * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		panning = false;
		yield return null;
	}
}

[System.Serializable]
public class CameraPanEvent {
	[SerializeField]
	public Vector3 cameraPosition;
	[SerializeField]
	public Collider triggerCollider;
	public float zoomCameraSize;
	public float panSpeed;
	public float lookDuration;
}
