using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharController : MonoBehaviour {

    public float moveSpeed = 1f;
    public float jumpForce = 500f;

    private Rigidbody2D r2d;

    bool facingRight = true;

    bool grounded = false;
    public LayerMask groundMask;
    public float groundRad = 0.2f;

    private Animator anim;

    private Enemy target;
    private float lastHit = 0;
    public float hitInterval = 0.2f;

    public int minDamage = 5, maxDamage = 15;

    public DamageText textPrefab;

    public GameObject breathBar;

    public AudioSource jump, fight;

	// Use this for initialization
	void Start () {
        r2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(transform.position, groundRad, groundMask);
        // If previously not grounded, but now grounded: Trigger
        if (anim.GetBool("Grounded") && !grounded) 
            anim.SetTrigger("GroundTrigger");
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("vSpeed", r2d.velocity.y);

        float dir = Input.GetAxis("Horizontal");
        anim.SetFloat("InputSpeed", Mathf.Abs(dir));

        if (grounded)
            r2d.velocity = new Vector2(dir * moveSpeed, r2d.velocity.y);
        else 
            r2d.AddForce(new Vector2(dir * moveSpeed / 1.5f, 0));
        anim.SetFloat("hSpeed", Mathf.Abs(r2d.velocity.x));

        if (dir > 0 && !facingRight)
            Flip();
        else if (dir < 0 && facingRight)
            Flip();
    }

    void Update() {
        if (grounded) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                anim.SetBool("Grounded", false);
                r2d.AddForce(new Vector2(0, jumpForce));

                jump.Play();
            }
            if (Input.GetKey(KeyCode.Z)) {
                anim.SetBool("Fight", true);
                if (!fight.isPlaying && target != null)
                    fight.Play();
            } else
                anim.SetBool("Fight", false);
        } else {
            anim.SetBool("Fight", false);
        }

        if (!anim.GetBool("Fight") || target == null)
            fight.Stop();

        // Attack
        if (anim.GetBool("Fight") && target != null) {
            if (Time.time > lastHit + hitInterval) {
                lastHit = Time.time;
                int dmg = Random.Range(minDamage, maxDamage);
                target.damage(dmg, transform.position + Vector3.up);

                DamageText dt = (DamageText) GameObject.Instantiate(textPrefab, target.transform.position + new Vector3(0, 1, -1), Quaternion.identity);
                dt.text = dmg + "";
            }
        }

        if (Input.GetKey(KeyCode.Escape)) {
            Restart();
        }
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;

        if (facingRight) {
            breathBar.transform.localScale = new Vector3(1, 1, 1);
        } else
            breathBar.transform.localScale = new Vector3(-1, 1, 1);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (target == null)
            target = collider.gameObject.GetComponent<Enemy>();
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (target != null && collider.gameObject == target.gameObject)
            target = null;
    }

    public void Restart() {
        PlayerPrefs.SetInt("restartCount", PlayerPrefs.GetInt("restartCount") + 1);
        PlayerPrefs.SetString("seenTutorial", "true");
        SceneManager.LoadScene("MainScene");
    }

    void OnApplicationQuit() {
        PlayerPrefs.SetInt("restartCount", 0);
        PlayerPrefs.SetString("seenTutorial", "false");
    }

}
