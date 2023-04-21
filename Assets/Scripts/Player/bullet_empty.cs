using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_empty : MonoBehaviour
{
    public Rigidbody2D rb;
    public float ejectForce = 5f;
    public float shellExistTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 ejectDirection = transform.up;
        rb.velocity = ejectDirection * ejectForce;
        rb.gravityScale = 1f;
        Destroy(gameObject, shellExistTime);
    }
}
