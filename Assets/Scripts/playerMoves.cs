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
    private float temp_jumpForce;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpMultiplier;
    
    Vector2 pGravity;

    //Player attack
    public bool isAiming;
    public bool isCrouching;
    public bool isFiring;

    void Start()
    {
        pGravity = new Vector2(0, -Physics2D.gravity.y);
        temp_speed = speed;
        temp_jumpForce = jumpForce;
        isFiring = false;
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
        jumpAnim();

        if (isAiming == true)
        {
            speed = 0;
            jumpForce = 0;
        }
        else if (isAiming == false)
        {
            speed = temp_speed;
            jumpForce = temp_jumpForce;
        }
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

    public void Fire(InputAction.CallbackContext context)
    {
        
        if (!isCrouching && isAiming && !isFiring && context.performed)
        {
            animator.SetBool("firing", true);
            isFiring = true;
            Debug.Log("Shoot!");
        }
        if(isCrouching && isAiming && !isFiring && context.performed)
        {
            animator.SetBool("crouch&firing", true);
            isFiring = true;
            Debug.Log("Crouch_Shoot!");
        }
        if (context.canceled)
        {
            isFiring = false;
            animator.SetBool("firing", false);
            animator.SetBool("crouch&firing", false);
        }
    }

    //player aims
    public void Aim(InputAction.CallbackContext context)
    {
        if (context.performed && hitGround)
        {
            animator.SetBool("aiming", true);
            isAiming = true;
        }
        if(context.canceled)
        {
            animator.SetBool("aiming", false);
            isAiming = false;
        }

        if((isCrouching && context.performed) || (context.performed && isCrouching))
        {
            animator.SetBool("crouching&aiming", true);
            isAiming = true;
        }
        if (!isCrouching && context.performed)
        {
            animator.SetBool("aiming", true);
            isAiming = true;
        }
        if (!isAiming)
        {
            animator.SetBool("aiming", false);
            animator.SetBool("crouching&aiming", false);
            isAiming = false;
        }
    }

    //player crouches
    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && hitGround)
        {
            isCrouching = true;
            animator.SetBool("crouching", true);
            speed *= 0.5f;

        }
        if (context.canceled)
        {
            isCrouching = false;
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
    public void jumpAnim()
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
        if (!isjumping) //lock player's direction when jumping
        {
            isFacingPositive = !isFacingPositive;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1.0f;
            transform.localScale = localScale;
        }
        
    }


}
