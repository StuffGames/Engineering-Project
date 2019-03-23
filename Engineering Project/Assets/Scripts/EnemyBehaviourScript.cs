using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour
{

    public CircleCollider2D triggerZone;
    public BoxCollider2D player;
    public CapsuleCollider2D headCheck;
    public CapsuleCollider2D playerFeet;
    public Animator anim;
    private Rigidbody2D rb;
    private Color enemyColor = Color.white;
    private SpriteRenderer sr;

    public bool destroyNow = false;
    private bool playerTrigger = false;
    private bool stomped = false;
    private bool isDead = false;
    private bool isAttacking = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {

        if (stomped)
        {
            isDead = true;
        }

        sr.color = enemyColor;

        if (!playerTrigger)
        {
            anim.SetBool("isDumb", true);
        }
        else if (playerTrigger)
        {
            triggerZone.radius = 10;
            anim.SetBool("isDumb", false);
            Vector3 dir = player.transform.position - transform.position;
            //Debug.Log(dir);
            if (dir.x > 0)
            {
                transform.rotation = new Quaternion(0f, 0f, 0f, 1.0f);
            }
            else if (dir.x < 0)
            {
                transform.rotation = new Quaternion(0f, 1.0f, 0f, 0f);
            }
            if (!isDead)
            {
                rb.velocity = new Vector2(transform.right.x * 2.5f, rb.velocity.y);
            }
        }

        /*if (isAttacking)
        {
            player.gameObject.SendMessage("Damage");
        }*/

        if (isDead)
        {
            //player.gameObject.SendMessage("EnemyBounce");
            anim.SetBool("isDead", true);
            if (transform.position.y < -200)
            {
                Destroy(gameObject);
            }
        }
        FlashingDead();
    }

    void FlashingDead()
    {
        if (isDead)
        {
            enemyColor.r = Mathf.PingPong(Time.time * 3, 1f);
            enemyColor.b = Mathf.PingPong(Time.time * 3, 1f);
            enemyColor.g = Mathf.PingPong(Time.time * 3, 1f);
        }
        else
        {
            enemyColor = Color.white;
        }
    }

    void FixedUpdate()
    {
        Physics2D.IgnoreCollision(playerFeet, GetComponent<BoxCollider2D>());
        playerTrigger = Physics2D.IsTouching(player, triggerZone);
        stomped = Physics2D.IsTouching(playerFeet, headCheck);
        isAttacking = Physics2D.IsTouching(player, GetComponent<BoxCollider2D>());

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = 2.5f;
        }
        else
        {
            rb.gravityScale = 2f;
        }

        if (stomped)
        {
            player.gameObject.SendMessage("EnemyBounce");
            rb.constraints = RigidbodyConstraints2D.None;
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2 (0, 300));
            headCheck.enabled = false;
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.enabled = false;
        }

    }
}
