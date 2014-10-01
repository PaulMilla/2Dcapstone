using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialText : MonoBehaviour {
    public Collider playerCollider;
	public Collider room1Collider;
    public Collider switchCollider1;
    public Collider buttonCollider1;
    public Collider switchCollider2;
    public Collider buttonCollider2;
	public Collider room4Collider;
	public Collider laserCollider;
	public Collider room5Collider;
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
		if (playerCollider.bounds.Intersects(room1Collider.bounds)) {
			Show();
			ChangeText("Buttons are there for a reason.  Why don't you try pressing it?");
			HideAfter(2);
		}
        else if (playerCollider.bounds.Intersects(switchCollider1.bounds))
        {
			Show();
            ChangeText("Hey you can press a switch! Not bad, but what about that button over there?");
            HideAfter(2);
        }
        else if (playerCollider.bounds.Intersects(buttonCollider1.bounds))
        {
            Show();
            ChangeText("Did you not bring anyone other than yourself? This is a multi-man operation!");
            HideAfter(2);
        }
        else if (playerCollider.bounds.Intersects(switchCollider2.bounds))
        {
            Show();
            ChangeText("Wait, what? What just happened?");
            HideAfter(2);
        }
        else if (playerCollider.bounds.Intersects(buttonCollider2.bounds))
        {
            Show();
            ChangeText("Ok, well it seems like you've got it from here. Make it out and you're hired!");
            HideAfter(2);
        }
		else if (room4Collider != null && playerCollider.bounds.Intersects(room4Collider.bounds)) {
			Show();
			ChangeText("Lasers are harmless. I promise.");
			Destroy(room4Collider);
			HideAfter(2);
		}
		else if (laserCollider != null && playerCollider.bounds.Intersects(laserCollider.bounds)) {
			Show();
			ChangeText("Just kidding. Luckily you can still rewind time after getting fried.");
			Destroy(laserCollider);
			HideAfter(2);
		}
		else if (playerCollider.bounds.Intersects(room5Collider.bounds)) {
			Show();
			ChangeText("You can only exist twice.  Use this to your advantage.");
			HideAfter(5);
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
