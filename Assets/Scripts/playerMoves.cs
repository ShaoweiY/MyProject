using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playerMoves : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D col;

    //Move speed & Face direction
    [SerializeField] float speed;
    float temp_speed;
    private float horizontal;
    private bool isFacingPositive = true;

    //Ground check
    public bool hitGround;
    public Transform onGround;
    public LayerMask groundLayer;
    public Animator animator;

    public float footOffset = 0.4f;
    public float headAbove = 0.5f;
    public float groundDist = 0.2f;

    //Collection
    public int Ammo = 0;
    public Text storageNum;

    //Jump
    [SerializeField] bool isjumping;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpForce;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpMultiplier;
    
    Vector2 pGravity;

    void Start()
    {
        pGravity = new Vector2(0, -Physics2D.gravity.y);
        temp_speed = speed;

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

        physicsCheck();
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

    //player reload
    public void Reload(InputAction.CallbackContext context)
    {
        if (Ammo > 0 && context.performed && hitGround)
        {
            animator.SetBool("reloading", true);
            Ammo--;
        }
    }

    //player aims
    public void Aim(InputAction.CallbackContext context)
    {
        if (context.performed && hitGround)
        {
            animator.SetBool("aiming", true);
            speed = 0;

            
        }
        if(context.canceled)
        {
            animator.SetBool("aiming", false);
            speed = temp_speed;

        }
        
    }

    //player crouches
    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && hitGround)
        {
            animator.SetBool("crouching", true);
            speed *= 0.5f;

        }
        if (context.canceled)
        {
            animator.SetBool("crouching", false);
            speed = temp_speed;

        }
    }

    //player jumps
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && hitGround)
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

    public void physicsCheck()
    {
        if (col.IsTouchingLayers(groundLayer))
        {
            hitGround = true;
            isjumping = false;
        }
        else
        {
            hitGround = false;
            isjumping = true;
        }
            
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
        else if (hitGround)
        {
            animator.SetBool("falling", false);
            animator.SetBool("idle", true);
        }
    }

    //change direction
    private void Flip()
    {
        if (!isjumping)
        {
            isFacingPositive = !isFacingPositive;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1.0f;
            transform.localScale = localScale;
        }
        
    }


}
