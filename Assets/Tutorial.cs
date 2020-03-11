using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject keyText, goalText, drownText, goodLuckText;
    public GameObject spawner, liquid;

    public float waitTime = 5;

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetString("seenTutorial") == "true") {
            keyText.SetActive(false);
            liquid.SetActive(true);
            spawner.SetActive(true);
            Component.Destroy(this);

        }
    }
	
	// Update is called once per frame
	void Update () {
		if (keyText.activeSelf) {
            if (Input.anyKey) {
                Invoke("enableGoalText", waitTime);
                Invoke("enableDrownText", 2 * waitTime);
                Invoke("enableGoodLuckText", 3 * waitTime);
                Invoke("hideText", 4 * waitTime);
            }
        }
	}

    void enableGoalText() {
        keyText.SetActive(false);
        goalText.SetActive(true);
        liquid.SetActive(true);
        spawner.SetActive(true);
    }

    void enableDrownText() {
        goalText.SetActive(false);
        drownText.SetActive(true);
    }

    void enableGoodLuckText() {
        drownText.SetActive(false);
        goodLuckText.SetActive(true);
    }

    void hideText() {
        goodLuckText.SetActive(false);
    }
}
