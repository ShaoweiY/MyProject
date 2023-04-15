using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 0.8f);
    }

    void FixedUpdate()
    {
        transform.Translate(transform.right * transform.localScale.x * speed);
    }

}
