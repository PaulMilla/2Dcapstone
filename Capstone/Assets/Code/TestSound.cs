using UnityEngine;
using System.Collections;

public class TestSound : MonoBehaviour {

	private AudioSource soundEffectAlert;
	private AudioSource soundEffectMotor;
	private AudioSource soundEffectMotorStart;
	private AudioSource[] soundDialoguesSeesPlayer;
	private AudioSource[] soundDialoguesBackToPatrol;
	private AudioSource[] soundDialoguesInvestigate;

	public bool testBackToPatrol = false;
	public bool testInvestigate = false;

	// Use this for initialization
	void Start () {
		soundEffectAlert = transform.transform.FindChild ("Alert").GetComponent<AudioSource> ();
		soundEffectMotor = transform.FindChild ("Motor").GetComponent<AudioSource> ();
		soundEffectMotorStart = transform.FindChild ("MotorStart").GetComponent<AudioSource> ();
		soundDialoguesSeesPlayer = transform.FindChild ("SeesPlayer").GetComponents<AudioSource> ();
		soundDialoguesBackToPatrol = transform.FindChild ("ReturnToPatrol").GetComponents<AudioSource> ();
		soundDialoguesInvestigate = transform.FindChild ("Investigating").GetComponents<AudioSource> ();
		soundEffectMotor.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if(testInvestigate) {
			playSoundInvestigating();
		} else if (testBackToPatrol) {
			playSoundBackToPatrol();
		}

		testInvestigate = false;
		testBackToPatrol = false;
	}

	public void playSoundAlert() {
		soundEffectAlert.Play ();
	}

	public void playSoundBackToPatrol() {
		int randomIndex = Random.Range (0, soundDialoguesBackToPatrol.Length - 1);
		soundDialoguesBackToPatrol [randomIndex].Play ();
	}
	
	public void playSoundInvestigating() {
		int randomIndex = Random.Range (0, soundDialoguesInvestigate.Length - 1);
		soundDialoguesInvestigate [randomIndex].Play ();
	}
}
