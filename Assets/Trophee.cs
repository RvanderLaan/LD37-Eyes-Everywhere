using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophee : MonoBehaviour {

    public GameObject victoryText;

    AudioSource audioSource;

    private bool won = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D c2d) {
        if (!won && c2d.gameObject.tag == "Player") {
            GetComponent<AudioSource>().Play();
            GameObject.Find("_GM").GetComponent<AudioSource>().Stop();
            victoryText.SetActive(true);
            won = true;
            victoryText.GetComponent<TextMesh>().text = "VICTORY! Total restarts: " + PlayerPrefs.GetInt("restartCount");
        }
    }
}
