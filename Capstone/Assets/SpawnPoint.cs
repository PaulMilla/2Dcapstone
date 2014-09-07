using UnityEngine;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour {

	[SerializeField]
	private GameObject objectToSpawn;
	private GameObject spawnedInstance { get; set; }

	public void Spawn() {
		Vector3 position = transform.position - objectToSpawn.transform.position;
		spawnedInstance = GameObject.Instantiate(objectToSpawn, position, transform.rotation) as GameObject;
	}
	public void DestroySpawnedObject() {
		if (spawnedInstance != null) {
			Destroy(spawnedInstance);
		}
	}
	void OnDrawGizmos() {
		Gizmos.color = (LayerMask.NameToLayer("Player") == objectToSpawn.layer) ? Color.green : Color.red;
		Gizmos.DrawSphere(transform.position, 1);
	}
}
