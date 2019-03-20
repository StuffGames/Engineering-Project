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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (!playerTrigger)
        {
            anim.SetBool("isDumb", true);
        }else if (playerTrigger)
        {
            triggerZone.radius = 10;
            anim.SetBool("isDumb", false);
            Vector3 dir = player.transform.position - transform.position;
            //Debug.Log(dir);
            if (dir.x < 0)
            {
                transform.Rotate(0,180,0, Space.Self);
                //Maybe do some trial and error for the right Quaternion value idk
            }
            rb.velocity = new Vector2(transform.right.x * 2.5f, 0f);
        }

        if (isDead)
        {
            player.gameObject.SendMessage("EnemyBounce");
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        playerTrigger = Physics2D.IsTouching(player, triggerZone);
        isDead = Physics2D.IsTouching(playerFeet, headCheck);
    }
}
