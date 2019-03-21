using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContollerScript : MonoBehaviour
{

    private Rigidbody2D rb;
    public Animator anim;
    public GameObject heartPrefab;
    public SpriteRenderer sr;
    public Transform canvasGM;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;
	public LayerMask whatIsWall;
    private GameManager gm;
    private Color someColor = new Color(1f, 1f, 1f, 1f);

    public float speed = 5f;
    public float jumpForce = 500f;
    private float radius = 0.1f;
    private int lives;
    private Vector2 size = new Vector2(0.71f,0.2f);
    public bool grounded = false;
	public bool wallTouch = false;
    private bool bouncing = false;
    private bool isInvincible = false;
    private bool startLevel = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        lives = 3;

        LivesUI();

        sr.color = someColor;

        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

    void Update()
    {
        int currentLives = lives;

        sr.color = someColor;

        anim.SetFloat("playerSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("airSpeed", rb.velocity.y);
        anim.SetBool("isGrounded", grounded);

        if (Input.GetKeyDown(KeyCode.K))
        {
            lives--;
            Debug.Log(lives);
        }

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
            gm.SendMessage("PlayerIsDead");
        }

        if (currentLives != lives)
        {
            LivesUI();
        }

        FlashingFrames(isInvincible);

    }

    private void FixedUpdate()
    {
        float playerRight = transform.right.x;

        if (startLevel)
        {
            rb.velocity = new Vector2(playerRight * speed, rb.velocity.y);
        }
        grounded = Physics2D.OverlapBox(groundCheck.position, size, 0, whatIsGround);
		wallTouch = Physics2D.OverlapCircle(wallCheck.position, radius, whatIsWall);

        if (grounded && Input.GetKey(KeyCode.Space))
        {
			rb.velocity = new Vector2 (rb.velocity.x,0);
			rb.AddForce(new Vector2(0, jumpForce));
            anim.SetBool("isJumping", true);
            if (!startLevel)
            {
                startLevel = true;
            }
        }
        else if (grounded)
        {
            anim.SetBool("isJumping", false);
            bouncing = false;
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
            if(transform.tag != "Canvas")
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
            StartCoroutine("InvincibilityFrames");
            lives--;
            LivesUI();
            Debug.Log(lives);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        //Debug.Log(col.transform.name);
    }

}
