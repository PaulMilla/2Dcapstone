using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUITextBox : MonoBehaviour {
    private Animator anim;
    private Text text;
    public Collider playerCollider;
    public TextEvent[] textEvents;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        text = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [System.Serializable]
    public class TextEvent
    {
        public string text;
        public Collider collider;
    }
}
