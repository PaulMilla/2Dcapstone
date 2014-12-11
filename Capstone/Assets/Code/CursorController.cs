using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {
	public Texture2D mouseCursor;
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100, 1 << LayerMask.NameToLayer ("Interactable"))) {
			Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);
		} else {
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}
}
