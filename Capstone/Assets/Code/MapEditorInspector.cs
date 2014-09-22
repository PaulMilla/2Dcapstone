using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(MapEditor))]
public class MapEditorInspector : Editor {

	public override void OnInspectorGUI() {

		MapEditor editor = (MapEditor)target;

		editor.floorTilePrototype = EditorGUILayout.ObjectField("Floor Tile: ", editor.floorTilePrototype, typeof(GameObject)) as GameObject;
		editor.center = EditorGUILayout.Vector3Field("Center: ", editor.center);
		editor.tileScale = EditorGUILayout.Vector2Field("Tile Scale: ", editor.tileScale);

		editor.xLength = EditorGUILayout.IntField("Length:", editor.xLength);
		editor.yLength = EditorGUILayout.IntField("Height:", editor.yLength);
		editor.gridRotation = EditorGUILayout.FloatField("Rotation:", editor.gridRotation);

		if (GUILayout.Button("Generate")) {
			GenerateTiles(editor);
		}
	}
	private void GenerateTiles(MapEditor editor) {
		if (editor.floorTilePrototype == null) {
			Debug.LogError("Assign floor tile prototype to map editor!");
			return;
		}
		Vector3 corner = editor.center;
		corner.x -= (editor.xLength * editor.tileScale.x / 2.0f) - editor.tileScale.x / 2.0f;
		corner.z -= (editor.yLength * editor.tileScale.y / 2.0f) - editor.tileScale.y / 2.0f;
		Vector3 tilePos = corner;
		GameObject empty = new GameObject();
		empty.name = "mapGrid";
		empty.transform.parent = editor.transform;
		empty.transform.position = editor.center;
		for (int i = 0; i < editor.xLength; i++) {
			for (int j = 0; j < editor.yLength; j++) {
				GameObject tile = GameObject.Instantiate(editor.floorTilePrototype, tilePos, editor.floorTilePrototype.transform.rotation) as GameObject;
				tile.transform.localScale = editor.tileScale;
				tile.transform.parent = empty.transform;
				tilePos.z += editor.tileScale.y;
			}
			tilePos.z = corner.z;
			tilePos.x += editor.tileScale.x;
		}
		empty.transform.localEulerAngles = new Vector3(empty.transform.localEulerAngles.x, empty.transform.localEulerAngles.y + editor.gridRotation, empty.transform.localEulerAngles.z);
	}
}
