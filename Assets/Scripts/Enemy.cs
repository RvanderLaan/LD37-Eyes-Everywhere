using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int maxHP = 100;
    private int HP;

    public float maxSpeed = 10;

    private Rigidbody2D r2d;
    private GameObject player;
    private Animator anim;

    public AudioSource deathPlayer;

    float lastPushTime;
    public float pushTimeInterval = 5;

    bool playerCollision = false;

    public AudioClip[] pushClips;
    AudioSource pushSource;
    

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        HP = maxHP;
        anim = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();

        pushSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if (HP > 0) {
            Vector2 playerDir = player.transform.position - transform.position;
            playerDir = playerDir.normalized * maxSpeed;
            playerDir.y = r2d.velocity.y;
            r2d.velocity = playerDir;
        }
        if (playerCollision && Time.time > lastPushTime + pushTimeInterval) {
            // Only push is player is slightly higher
            if (player.transform.position.y > transform.position.y + 0.3f) {
                lastPushTime = Time.time;
                Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
                playerRB.AddForce(new Vector2(Random.Range(-3f, 3f), 0.5f) * Random.Range(100, 150));

                pushSource.clip = pushClips[Random.Range(0, pushClips.Length)];
                pushSource.pitch = Random.Range(0.7f, 1.3f);
                pushSource.Play();
            }
        }
    }

    public void damage(int amount, Vector2 source) {
        HP -= amount;
        anim.SetInteger("HP", HP);

        Vector2 force = transform.position;
        force -= source;
        force = force.normalized * amount * 10;

        // Push back
        if (HP < 0) {
            r2d.AddForce(10 * force);
            // Ragdoll mode
            r2d.constraints = RigidbodyConstraints2D.None;
            deathPlayer.pitch = Random.Range(0.8f, 1.2f);
            deathPlayer.Play();

            

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
                Vector2 dist = (go.transform.position - transform.position);
                float magn = dist.magnitude;
                magn = -magn + 10;
                if (magn > 0)
                    go.GetComponent<Rigidbody2D>().AddForce(dist.normalized * magn * 50);
            }

            Component.Destroy(this);

        } else {
            r2d.AddForce(force);
        }

    }

    void OnCollisionEnter2D(Collision2D c2d) {
        if (c2d.gameObject.tag == "Player") {
            playerCollision = true;
        }
    }

    void OnCollisionExit2D(Collision2D c2d) {
        if (c2d.gameObject.tag == "Player") {
            playerCollision = false;
        }
    }
}
