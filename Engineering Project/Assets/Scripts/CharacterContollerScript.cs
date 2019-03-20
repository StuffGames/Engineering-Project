using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContollerScript : MonoBehaviour
{

    private Rigidbody2D rb;
    public Animator anim;
    public Transform canvasGM;
    public GameObject heartPrefab;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;
	public LayerMask whatIsWall;

    public float speed = 5f;
    public float jumpForce = 500f;
    private float radius = 0.1f;
    private int lives;
    private Vector2 size = new Vector2(0.78f,0.2f);
    public bool grounded = false;
	public bool wallTouch = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lives = 3;

        LivesUI();
    }

    // Update is called once per frame
    void Update()
    {
        int currentLives = lives;

        anim.SetFloat("playerSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("airSpeed", rb.velocity.y);
        anim.SetBool("isGrounded", grounded);

        if (Input.GetKeyDown(KeyCode.K))
        {
            lives--;
            Debug.Log(lives);
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
            Debug.Log("Game Over");
            Time.timeScale = 0;
        }

        if (currentLives != lives)
        {
            LivesUI();
        }

    }

    private void FixedUpdate()
    {
        float playerRight = transform.right.x;
        rb.velocity = new Vector2(playerRight * speed, rb.velocity.y);
        grounded = Physics2D.OverlapBox(groundCheck.position, size, 0, whatIsGround);

		wallTouch = Physics2D.OverlapCircle(wallCheck.position, radius, whatIsWall);

        if (grounded && Input.GetKey(KeyCode.Space))
        {
			rb.velocity = new Vector2 (rb.velocity.x,0);
			rb.AddForce(new Vector2(0, jumpForce));
            anim.SetBool("isJumping", true);
        }
        else if (grounded)
        {
            anim.SetBool("isJumping", false);
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
            GameObject newHeart = Instantiate(heartPrefab, new Vector3 (-200 + (100 * i),820,0), Quaternion.identity, canvasGM);
            newHeart.name = "Heart " + i;
        }
    }

}
