using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {

    public float riseSpeed = 1f;
    public float lifeTime = 2f;
    public string text;

	// Use this for initialization
	void Start () {
        GameObject.Destroy(gameObject, lifeTime);

        GetComponent<TextMesh>().text = this.text;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.y += Time.deltaTime * riseSpeed;
        transform.position = pos;
	}
}
