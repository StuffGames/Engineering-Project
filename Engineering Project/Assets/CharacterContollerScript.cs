using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContollerScript : MonoBehaviour
{

    private Rigidbody2D rb;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;

    public float speed = 5f;
    public float jumpForce = 500f;
    private float radius = 0.3f;
    private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        float playerRight = transform.right.x;
        rb.velocity = new Vector2(playerRight * speed, rb.velocity.y);
        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);

        if (grounded && Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
        }
    }
}
