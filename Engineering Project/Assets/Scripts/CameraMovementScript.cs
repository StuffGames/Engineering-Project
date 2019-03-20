using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float smoothTime;

    private Vector3 vel = Vector3.zero;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref vel, smoothTime);
    }
}
