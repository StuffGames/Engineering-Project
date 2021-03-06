﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContollerScript : MonoBehaviour
{

    public AudioSource jumpingSound;
    public AudioSource hitSound;
    public AudioSource deathSound;

    private Rigidbody2D rb;
    public Animator anim;
    public GameObject heartPrefab;
    public SpriteRenderer sr;
    private Transform canvasGM;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;
	public LayerMask whatIsWall;
    private GameManager gm;
    private Color someColor = new Color(1f, 1f, 1f, 1f);
    public GameObject youDied;
    public GameObject pressToStart;
    //private CircleCollider2D extraLife;

    public float speed = 5f;
    public float jumpForce = 500f;
    private float radius = 0.1f;
    public int lives;
    private Vector2 size = new Vector2(0.71f,0.2f);
    public bool grounded = false;
	public bool wallTouch = false;
    private bool bouncing = false;
    private bool isInvincible = false;
    private bool startLevel = false;
    private bool hitBack = false;
    private bool doubleJump = true;

    private float xPos;
    private float yPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canvasGM = GameObject.FindGameObjectWithTag("Canvas").transform;
        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        lives = 3;

        LivesUI();

        sr.color = someColor;

        Physics2D.IgnoreLayerCollision(8, 9, false);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    void Awake()
    {
        canvasGM = GameObject.FindGameObjectWithTag("Canvas").transform;
        lives = 3;
        LivesUI();
    }

    bool jumpRequest = false;
    bool soundOnce = true;
    void Update()
    {
        //Debug.Log("(" + xPos + ", "+ yPos +")");

        int currentLives = lives;

        sr.color = someColor;

        anim.SetFloat("playerSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("airSpeed", rb.velocity.y);
        anim.SetBool("isGrounded", grounded);

        if (transform.position.y < -500)
        {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequest = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpRequest = false;
        }

        /*if (Input.GetKeyDown(KeyCode.L))
        {
            lives++;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            lives--;
        }*/

        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.Rotate(0,180,0, Space.Self);
        }
        if (Input.GetKeyDown(KeyCode.R))
		{
			transform.position = new Vector2 (0,0);
            rb.velocity = new Vector2(0, 0);
		}

		if (wallTouch)
		{
			transform.Rotate (0,180,0, Space.Self);
			wallTouch = false;
		}

        if (lives < 1)
        {
            anim.SetBool("isDead", true);
            if (!deathSound.isPlaying && soundOnce)
            {
                deathSound.Play();
                soundOnce = false;
            }
            rb.constraints = RigidbodyConstraints2D.None;
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
            groundCheck.gameObject.SetActive(false);
            wallCheck.gameObject.SetActive(false);
            box.enabled = false;
            capsule.enabled = false;
            rb.AddTorque(50);
            CameraMovementScript mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementScript>();
            mainCam.isPlayerDead = true;
            youDied.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gm.SendMessage("PlayerIsDead");
            }
        }

        if (currentLives != lives)
        {
            LivesUI();
        }

        FlashingFrames(isInvincible);
    }

    bool hitReady = false;

    private void FixedUpdate()
    {
        float playerRight = transform.right.x;

        if (startLevel)
        {
            if (!hitBack)
            {
                rb.velocity = new Vector2(playerRight * speed, rb.velocity.y);
            }
        }

        if (lives > 0)
        {
            grounded = Physics2D.OverlapBox(groundCheck.position, size, 0, whatIsGround);
            wallTouch = Physics2D.OverlapCircle(wallCheck.position, radius, whatIsWall);
        }

        if (!grounded)
        {
            hitReady = true;
        }
        else if (grounded)
        {
            if (hitReady)
            {
                hitBack = false;
                hitReady = false;
            }
        }
        if (grounded && jumpRequest)
        {
            if (!jumpingSound.isPlaying)
            {
                jumpingSound.Play();
            }
            xPos = transform.position.x;
			rb.velocity = new Vector2 (rb.velocity.x,0);
			rb.AddForce(new Vector2(0, jumpForce));
            anim.SetBool("isJumping", true);
            if (!startLevel)
            {
                startLevel = true;
                pressToStart.SetActive(false);
            }
            jumpRequest = false;
            yPos = transform.position.y;
        }
        else if (grounded)
        {
            anim.SetBool("isJumping", false);
            bouncing = false;
            doubleJump = true;
        }

        if (doubleJump && !grounded && jumpRequest)
        {
            jumpingSound.Play();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce));
            doubleJump = false;
            jumpRequest = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = 2.5f;
        }
        else
        {
            rb.gravityScale = 2;
        }
    }

    public void LivesUI()
    {
        foreach (Transform heart in canvasGM)
        {
            if(heart.tag == "Heart")
            {
                Destroy(heart.gameObject);
            }
        }

        for (int i = lives; i > 0; i--)
        {
            GameObject newHeart = Instantiate(heartPrefab, canvasGM);
            RectTransform heartRect = newHeart.GetComponent<RectTransform>();
            heartRect.anchoredPosition = new Vector2(-200 + (100 * i), 100);
            newHeart.name = "Heart " + i;
        }
    }

    public void EnemyBounce()
    {
        bouncing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce));
    }

    public void FlashingFrames(bool isInvincible)
    {
        if (isInvincible)
        {
            someColor.a = Mathf.PingPong(Time.time * 3, 1);
        }
        else if (!isInvincible)
        {
            someColor.a = 1;
        }
    }

    public IEnumerator InvincibilityFrames()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        anim.SetBool("isHit", true);
        isInvincible = true;
        yield return new WaitForSecondsRealtime(2);
        anim.SetBool("isHit", false);
        Physics2D.IgnoreLayerCollision(8, 9, false);
        isInvincible = false;
    }

    /*public void Damage()
    {
        if (!bouncing)
        {
            Debug.Log("thing");
            StartCoroutine("InvincibilityFrames");
            Debug.Log("after 2 seconds?");
            lives--;
            LivesUI();
            Debug.Log(lives);
        }
    }*/

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && !bouncing)
        {
            hitSound.Play();
            hitBack = true;
            rb.velocity = new Vector2(0, 0);
            if (lives < 2)
            {
                rb.AddForce(new Vector2(350 * -transform.forward.z, 300));
            }
            else
            {
                rb.AddForce(new Vector2(200 * -transform.forward.z, 300));
            }
            StartCoroutine("InvincibilityFrames");
            lives--;
            LivesUI();
            Debug.Log(lives);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ExtraLife")
        {
            lives++;
            LivesUI();
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "KillPlane")
        {
            lives = 0;
        }
    }

}
