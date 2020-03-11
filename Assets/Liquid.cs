using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour {

    public float riseSpeed = 0.1f;


    public int drownTime = 5;

    float drownStart = float.MaxValue;
    bool drowning = false;

    public GameObject breathBar, breathPivot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        breathBar.SetActive(drowning);
        

        Vector3 pos = transform.position;
        pos.y += riseSpeed * Time.deltaTime * (0.5f + 0.5f * Mathf.Sin(Time.time * Mathf.PI * 2));
        transform.position = pos;


        if (drowning) {
            Vector3 scal = breathPivot.transform.localScale;
            scal.x = 4 - 4 * (Time.time - drownStart) / drownTime;
            breathPivot.transform.localScale = scal;



            if (Time.time > drownStart + drownTime) {
                CharController c = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
                GetComponent<AudioSource>().Play();

                GameObject.Find("_GM").GetComponent<AudioSource>().Stop();
                c.Invoke("Restart", 2);

                drowning = false;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D c2d) {
        if (c2d.gameObject.name == "Head") {
            drowning = true;
            drownStart = Time.time;
        }
    }


    void OnTriggerExit2D(Collider2D c2d) {
        if (c2d.gameObject.name == "Head") {
            drowning = false;
        }
    }
}
