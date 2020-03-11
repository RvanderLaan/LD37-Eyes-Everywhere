using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour {

    CharController player;

    Vector3 target;

    public float speed = .1f;
    float startTime;

    public float timeout = 3;

    public Transform pivot;

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();

        target = player.transform.position + Vector3.up / 2;
        startTime = Time.time;

        sr = GetComponent<SpriteRenderer>();

        Vector3 dDist = target - transform.position;
        pivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dDist.y, dDist.x) * Mathf.Rad2Deg);
    }
	
	// Update is called once per frame
	void Update () {

        float dT = Time.time - startTime;
        if (dT > timeout)
            GameObject.Destroy(pivot.gameObject);

        

        Vector3 scal = pivot.transform.localScale;
        scal.x += Time.deltaTime * speed;
        pivot.transform.localScale = scal;

        sr.material.color = Color.HSVToRGB(0.5f + Mathf.Sin(dT * Mathf.PI) / 2, 1, 1);
	}

    void OnTriggerEnter2D(Collider2D c2d) {
        if (c2d.gameObject.tag == "Player") {
            Debug.Log("Ouch!");
            GameObject.Destroy(pivot.gameObject);
        }
    }
}
