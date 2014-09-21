using UnityEngine;
using System.Collections;

public class MapEditor : MonoBehaviour {

	public Vector3 center;
	public int xLength;
	public int yLength;
	public float gridRotation;
	public Vector2 tileScale;

	public GameObject floorTilePrototype;


	void Awake() {
		Destroy(this);
	}
	void OnDrawGizmosSelected() {
		float halfLength = xLength * tileScale.x / 2.0f;
		float halfWidth = yLength * tileScale.y / 2.0f;
		Vector2 offset = RotateAroundCenter(new Vector2(halfLength, halfWidth), gridRotation);
		Vector3 topRight = new Vector3(center.x + offset.x, center.y, center.z + offset.y);
		offset = RotateAroundCenter(new Vector2(halfLength, -halfWidth), gridRotation);
		Vector3 bottomRight = new Vector3(center.x + offset.x, center.y, center.z + offset.y);
		offset = RotateAroundCenter(new Vector2(-halfLength, -halfWidth), gridRotation);
		Vector3 bottomLeft = new Vector3(center.x + offset.x, center.y, center.z + offset.y);
		offset = RotateAroundCenter(new Vector2(-halfLength, halfWidth), gridRotation);
		Vector3 topLeft = new Vector3(center.x + offset.x, center.y, center.z + offset.y);
		Gizmos.DrawLine(topLeft, topRight);
		Gizmos.DrawLine(topLeft, bottomLeft);
		Gizmos.DrawLine(topRight, bottomRight);
		Gizmos.DrawLine(bottomRight, bottomLeft);
	}
	Vector2 RotateAroundCenter(Vector2 point, float degrees) {
		float x = point.x * Mathf.Cos(Mathf.Deg2Rad * degrees) - point.y * Mathf.Sin(Mathf.Deg2Rad * degrees);
		float y = point.x * Mathf.Sin(Mathf.Deg2Rad * degrees) + point.y * Mathf.Cos(Mathf.Deg2Rad * degrees);
		return new Vector2(x, y);
	}
}
