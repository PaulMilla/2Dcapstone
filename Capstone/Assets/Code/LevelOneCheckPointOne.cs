using UnityEngine;
using System.Collections;

public class LevelOneCheckPointOne : MonoBehaviour {

	public Camera MainCamera;

	Vector3 cameraInitialPosition;
	public Transform cameraStopTransform;

	bool triggered = false;
	bool notDone = true;

	public float cameraMoveSpeed;
	public PlayerMovement player;

	enum STAGE {
		STAGE_ONE,
		STAGE_TWO,
		STAGE_FINISH
	}

	private STAGE currentStage = STAGE.STAGE_ONE;

	private float timer = 0.0f;
	private float stageOneTime = 3.0f;
	private float stageTwoTime = 3.0f;

	// Update is called once per frame
	void Update () {
		if (triggered) {
			timer += Time.deltaTime;
			switch (currentStage) {
			case STAGE.STAGE_ONE:

				StageOneUpdate();
				break;
			case STAGE.STAGE_TWO:
				StageTwoUpdate();
				break;
			case STAGE.STAGE_FINISH:
				FinishCheckpoint();
				break;
			}
		}
	}

	void StageOneUpdate() {
		Debug.Log("Stage One");
		if (timer >= stageOneTime) {
			currentStage = STAGE.STAGE_TWO;
			timer = 0.0f;
		}
		MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, cameraStopTransform.position, cameraMoveSpeed);
		player.GetAgent().Stop();
	}

	void StageTwoUpdate() {
		Debug.Log("Stage Two");
		player.GetAgent().Stop();
		if (timer >= stageTwoTime) {
			currentStage = STAGE.STAGE_FINISH;
			timer = 0.0f;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals("Player")) {
			Debug.Log("Player Entered");
			if (!triggered) {
				currentStage = STAGE.STAGE_ONE;
				triggered = true;
				player.enabled = false;
				player.movementEnabled = false;
				player.GetAgent().Stop();
				cameraInitialPosition = MainCamera.transform.position;
				MainCamera.GetComponent<CameraFollow>().enabled = false;
			}
		}
	}

	void FinishCheckpoint() {
		Debug.Log("Stage Finish");
		//MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, cameraInitialPosition, cameraMoveSpeed);
		player.enabled = true;
		player.movementEnabled = true;
		MainCamera.GetComponent<CameraFollow>().enabled = true;
	}
}
