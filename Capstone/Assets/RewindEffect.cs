using UnityEngine;
using System.Collections;

public class RewindEffect : MonoBehaviour {

	MotionBlur motionBlur;
	NoiseEffect noiseEffect;

	// Use this for initialization
	void Start () {
		motionBlur = this.GetComponent<MotionBlur>();
		noiseEffect = this.GetComponent<NoiseEffect>();
		PlayerMovement.Instance.RewindBegin += () => {
			applyEffects();
		};
		PlayerMovement.Instance.RewindEnd += () => {
			removeEffects();
		};
	}

	void applyEffects() {
		if (motionBlur != null) {
			motionBlur.enabled = true;
		}
		if (noiseEffect != null) {
			noiseEffect.enabled = true;
			noiseEffect.monochrome = !noiseEffect.monochrome;
		}
	}

	void removeEffects() {
		if (motionBlur != null) {
			motionBlur.enabled = false;
		}
		if (noiseEffect != null) {
			noiseEffect.enabled = false;
		}
	}
}
