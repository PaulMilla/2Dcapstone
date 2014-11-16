﻿using UnityEngine;
using System.Collections;

public class ClickLocationIndicator : MonoBehaviour {
	public static ClickLocationIndicator Instance {
		get;
		private set;
	}

	Animator animator;

	void Awake() {
		Instance = this;
		animator = GetComponent<Animator>();
	}


	public void MoveTo (Vector3 indicatorPos, bool animate)
	{
		this.renderer.enabled = true;
		this.transform.position = indicatorPos;
		if(animate) {
			this.animator.Play("Click", 0, 0.0f);
		}
	}
}
