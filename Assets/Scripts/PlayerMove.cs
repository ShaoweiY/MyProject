using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public Rigidbody2D rb;
    public float speed = 10.0f;
    public float jumpForce;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float HMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
        float y = 0.25f;
        float x = 0.25f;

        if (HMove != 0)
        {
            rb.velocity = new Vector2(HMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            animator.SetFloat("running", Mathf.Abs(faceDirection));
        }

        if (faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection * x, 1 * y, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
        }

        Crouch();
        Aim();

    }

    void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            animator.SetBool("crouching", true);
        }else if (Input.GetButtonUp("Crouch"))
        {
            animator.SetBool("crouching", false);
        }
    }

    void Aim()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            animator.SetBool("aiming", true);
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            animator.SetBool("aiming", false);
        }
    }
}
