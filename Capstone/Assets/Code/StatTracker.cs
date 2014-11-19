using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatTracker : MonoBehaviour {

	private string timeKey = "time";
	private string trueTimeKey = "trueTime";
	private string timesCaughtKey = "timesCaught";

	float time = 0;
	float trueTime = 0;
	float timesCaught = 0;
	float rewindBeginTime = 0;

	[SerializeField]
	GameObject statTrackerPanel;

	[SerializeField]
	Text yourTimeText;
	[SerializeField]
	Text yourTrueTimeText;
	[SerializeField]
	Text yourTimesCaughtText;
	[SerializeField]
	Text bestTimeText;
	[SerializeField]
	Text bestTrueTimeText;
	[SerializeField]
	Text bestTimesCaughtText;

	// Use this for initialization
	void Start () {
		timeKey += Application.loadedLevelName;
		trueTimeKey += Application.loadedLevelName;
		timesCaughtKey += Application.loadedLevelName;
		
		PlayerMovement.Instance.RewindBegin += () => {
			rewindBeginTime = Time.time;
		};
		PlayerMovement.Instance.RewindEnd += () => {
			trueTime += Time.time - rewindBeginTime;
		};
		PlayerMovement.Instance.GetComponent<CharacterStatus>().Died += (dead) => {
			if (dead) timesCaught++;
		};
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ShowStats() {	
		time = Time.time;
		trueTime = Time.time - trueTime;

		float bestTime;
		float bestTrueTime;
		float bestTimesCaught;

		if (PlayerPrefs.HasKey(timeKey) && PlayerPrefs.GetFloat(timeKey) > 0) {
			bestTime = Mathf.Min(time, PlayerPrefs.GetFloat(timeKey));
			bestTrueTime = Mathf.Min(PlayerPrefs.GetFloat(trueTimeKey));
			bestTimesCaught = Mathf.Min(PlayerPrefs.GetFloat(timesCaughtKey));
		}
		else {
			bestTime = time;
			bestTrueTime = trueTime;
			bestTimesCaught = timesCaught;
		}


		yourTimeText.text = FormatTime(time);
		yourTrueTimeText.text = FormatTime(trueTime);
		yourTimesCaughtText.text = ""+(int)timesCaught;
		bestTimeText.text = FormatTime(bestTime);
		bestTrueTimeText.text = FormatTime(bestTrueTime);
		bestTimesCaughtText.text = ""+(int)bestTimesCaught;
	
		statTrackerPanel.SetActive(true);

		PlayerPrefs.SetFloat(timeKey, bestTime);
		PlayerPrefs.SetFloat(trueTimeKey, bestTrueTime);
		PlayerPrefs.SetFloat(timesCaughtKey, bestTimesCaught);
		PlayerPrefs.Save();
	}

	string FormatTime(float time) {
		if (time % 60 >= 10)
			return (int)time / 60 + ":" + (int)(time % 60);
		return (int)time / 60 + ":0" + (int)(time % 60);
	}

	public void LoadNextLevel() {
		GameManager.Instance.LoadNextLevel();
	}
}
