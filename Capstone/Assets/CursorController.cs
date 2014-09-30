using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

	public LensFlare flarePrefab;
	private LensFlare flare;
	
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100, 1 << LayerMask.NameToLayer ("Interactable"))) {
			if (flare == null) {
				Debug.Log("Set Flare");
				flare = Instantiate(flarePrefab, hit.point, Quaternion.identity) as LensFlare;
			} else {
				flare.transform.position = hit.point;
			}
		} else {
			if (flare != null) {
				GameObject.Destroy(flare.gameObject);
			}
		}
	}
}
