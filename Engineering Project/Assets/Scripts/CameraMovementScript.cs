using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float smoothTime;
    public bool isPlayerDead = false;

    private Vector3 vel = Vector3.zero;

    void Start()
    {
        isPlayerDead = false;
    }

    void FixedUpdate()
    {
        if (!isPlayerDead)
        {
            if (target.rotation.y < -0.9)
            {
                if (offset.x > 0)
                {
                    offset.x = -offset.x;
                }
            }
            else if (target.rotation.y > -1)
            {
                offset.x = Mathf.Abs(offset.x);
            }
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref vel, smoothTime);
        }
    }
}
