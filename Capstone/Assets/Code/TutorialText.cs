using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialText : MonoBehaviour {
    public Collider playerCollider;
    public Collider[] colliders;
    public string[] texts;
    Animator dialog;
    Text text;
    private float hideTime;
    float HideTime {
        get {
            return hideTime;
        }
        set {
            hideTime = value;
            if (hideTime <= 0) {
                Hide();
                hideTime = 0.0f;
            }
        }
    }

	// Use this for initialization
	void Start () {
        dialog = GetComponent<Animator>();
        text = GetComponentInChildren<Text>();
        dialog.enabled = true;
        Show();
	}
	
	// Update is called once per frame
	void Update () {
        for(int x = 0; x < colliders.Length; ++x) {
            if (playerCollider.bounds.Intersects(colliders[x].bounds)) {
                Show();
                ChangeText(texts[x]);
                HideAfter(3.5f);
            }
        }
	}

    void FixedUpdate()
    {
        if (HideTime > 0) {
            HideTime = HideTime - Time.fixedDeltaTime;
        }
    }

    public void Show() {
        dialog.SetBool("isHidden", false);
    }

    public void Hide() {
        dialog.SetBool("isHidden", true);
    }

    public void HideAfter(float f)
    {
        HideTime = f;
    }

    public void ChangeText(string s) {
        text.text = s;
    }
}

public class dialogEvent {
    public Collider collider;
    public string text;
    public dialogEvent(Collider c, string s) {
        collider = c;
        text = s;
    }
}