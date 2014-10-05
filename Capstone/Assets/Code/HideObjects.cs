using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HideObjects : MonoBehaviour {

	private Transform prevTransObject;
	public Color alphaValue = 0.5f; // our alpha value
	public List<int> transparentLayers = new List<int>();   // transparency layers.
	public float maxDistance = 20f; // Max distance from target to camera object.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
