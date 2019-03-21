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

    private bool playerTrigger = false;
    private bool isDead = false;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

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
            rb.velocity = new Vector2(transform.right.x * 2.5f, 0f);
        }

        /*if (isAttacking)
        {
            player.gameObject.SendMessage("Damage");
        }*/

        if (isDead)
        {
            player.gameObject.SendMessage("EnemyBounce");
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Physics2D.IgnoreCollision(playerFeet, GetComponent<BoxCollider2D>());
        playerTrigger = Physics2D.IsTouching(player, triggerZone);
        isDead = Physics2D.IsTouching(playerFeet, headCheck);
        isAttacking = Physics2D.IsTouching(player, GetComponent<BoxCollider2D>());
    }
}
