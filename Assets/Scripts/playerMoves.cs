using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playerMoves : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform onGround;
    public LayerMask groundLayer;
    public Animator animator;

    private float horizontal;    
    private bool isFacingPositive = true;
    public Collider2D col;

    public int Ammo = 0;
    public Text storageNum;

    [SerializeField] float speed;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpForce;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpMultiplier;
    float tempSpeed;
    Vector2 pGravity;

    void Start()
    {
        pGravity = new Vector2(0, -Physics2D.gravity.y);
        tempSpeed = speed;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (!isFacingPositive && horizontal > 0f)
        {
            Flip();
        }
        else if(isFacingPositive && horizontal < 0f)
        {
            Flip();
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity -= pGravity * fallMultiplier * Time.deltaTime;
        }

        switchAnim();
    }

    //object collection
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ammo")
        {
            Destroy(collision.gameObject);
            Ammo += 1;
            storageNum.text = Ammo.ToString();
        }
    }

    //player aims
    public void Aim(InputAction.CallbackContext context)
    {
        
        if (context.performed && col.IsTouchingLayers(groundLayer))
        {
            animator.SetBool("aiming", true);
            speed = 0;
        }
        if(context.canceled)
        {
            animator.SetBool("aiming", false);
            speed = tempSpeed;
        }
        
    }

    //player crouches
    public void Crouch(InputAction.CallbackContext context)
    {
        if(context.performed && col.IsTouchingLayers(groundLayer))
        {
            animator.SetBool("crouching", true);
            speed *= 0.5f;
        }
        if (context.canceled)
        {
            animator.SetBool("crouching", false);
            speed = tempSpeed;
        }
    }

    //player jumps
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && col.IsTouchingLayers(groundLayer))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("jumping", true);
        }
    }   

    //player moves
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        animator.SetFloat("running", Mathf.Abs(horizontal));
        
    }


    //change animation stages
    public void switchAnim()
    {
        animator.SetBool("idle", false);
        if (animator.GetBool("jumping"))
        {
            if(rb.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
        }
        else if (col.IsTouchingLayers(groundLayer))
        {
            animator.SetBool("falling", false);
            animator.SetBool("idle", true);
        }
    }

    //change direction
    private void Flip()
    {
        isFacingPositive = !isFacingPositive;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1.0f;
        transform.localScale = localScale;
    }
}
