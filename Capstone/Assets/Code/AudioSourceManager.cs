using UnityEngine;
using System.Collections;

/*
 * A utility class for modifying mulitple audiosource attached ton one game object at once 
 */
[ExecuteInEditMode]
public class AudioSourceManager : MonoBehaviour {

	private AudioSource[] audioSources;

	public float volume = 1.0f;
	public int spread = 0;
	public float minDistance = 1.0f;
	public float maxDistance = 10f;

	// Use this for initialization
	void Start () {
		audioSources = this.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (AudioSource audioSource in audioSources) {
				audioSource.volume = Mathf.Clamp (volume, 0f, 1f);
				audioSource.spread = (int) Mathf.Clamp (spread, 0, 360);
				audioSource.minDistance = Mathf.Clamp (minDistance, 0.0f, maxDistance - .05f);
				audioSource.maxDistance = Mathf.Clamp (maxDistance, minDistance +.05f, Mathf.Infinity);
		}
	}
}
