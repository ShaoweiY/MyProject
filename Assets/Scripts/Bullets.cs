using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    private bool isFacingPositive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 0.8f);
  //      rb.velocity = transform.right * speed;
    }

    void FixedUpdate()
    {
        rb.velocity = transform.right * speed;

        if (!isFacingPositive)
        {
            Flip();
        }
        else if (isFacingPositive)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingPositive = !isFacingPositive;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
