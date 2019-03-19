using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContollerScript : MonoBehaviour
{

    private Rigidbody2D rb;
    public Animator anim;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;
	public LayerMask whatIsWall;

    public float speed = 5f;
    public float jumpForce = 500f;
    private float radius = 0.1f;
    private Vector2 size = new Vector2(0.78f,0.2f);
    public bool grounded = false;
	public bool wallTouch = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        anim.SetFloat("playerSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("airSpeed", rb.velocity.y);

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
}
