using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<Enemy> enemyPrefabs;

    public float spawnInterval = 5;

    public float minX = 0, maxX = 10;
    public float moveCycle = 10;

    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", 0, spawnInterval);
	}

    void Spawn() {
        Enemy rndm = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        Enemy instance = (Enemy) GameObject.Instantiate(rndm, transform.position, Quaternion.identity);
        instance.deathPlayer = audioSource;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(minX, maxX, (Mathf.Sin(Time.time * 2 * Mathf.PI / moveCycle) * 0.5f + 0.5f));
        transform.position = pos;
	}
}
