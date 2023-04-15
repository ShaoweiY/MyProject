using System;
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
    public bool isFacingPositive = true;

    //Ground check
    public bool hitGround;
    public Transform onGround;
    public LayerMask groundLayer;
    public Animator animator;
    public LayerMask blocksLayer;

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
    public Transform shootPoint_std;
    public Transform shootPoint_crh;
    public GameObject bullet;

    //player health
    public int maxHealth = 100;
    public int current_health;
    public HealthBar health_bar;
    public int demage;

    //Ray
    [SerializeField] private float raycastLength;

    //climb
    //public bool ledgeDetected;
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;
    public bool isTouchingBlocks;
    

    //Sound effects
    [SerializeField] AudioSource fireEffect;
    [SerializeField] AudioSource stepsEffect;


    void Start()
    {
        current_health = maxHealth;
        health_bar.SetMaxHealth(maxHealth);

        pGravity = new Vector2(0, -Physics2D.gravity.y);
        temp_speed = speed;
        temp_jumpForce = jumpForce;
        isFiring = false;
        isTouchingBlocks = false;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (!isFacingPositive && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingPositive && horizontal < 0f)
        {
            Flip();
        }


        if (rb.velocity.y < 0)
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



    public void Hurt(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            System.Random random = new System.Random();
            demage = random.Next(0, 20);
            current_health -= demage;

            health_bar.setHealth(current_health);

            Debug.Log(current_health);
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

        if(collision.tag == "Medical")
        {
            Destroy(collision.gameObject);
            current_health += 30;
            health_bar.setHealth(current_health);
            
            if (current_health >= maxHealth)
            {
                current_health = maxHealth;
                health_bar.setHealth(current_health);
            }
            Debug.Log(current_health);
        }
    }

    //player reload
    public void Reload(InputAction.CallbackContext context)
    {
        animator.SetBool("reloading", true);
        Ammo--;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        
        if (!isCrouching && isAiming && !isFiring && context.performed)
        {
            fireEffect.Play();
            animator.SetBool("firing", true);
            if(isFacingPositive)
                Instantiate(bullet, shootPoint_std.position, transform.rotation);
            if (!isFacingPositive)
            {
                GameObject bulletInstance = Instantiate(bullet, shootPoint_std.position, transform.rotation);
                bulletInstance.transform.localScale = new Vector3(-0.007190318f, 0.008811175f, 1);
            }
                

            isFiring = true;
        }
        if(isCrouching && isAiming && !isFiring && context.performed)
        {
            fireEffect.Play();
            animator.SetBool("crouch&firing", true);

            if (isFacingPositive)
                Instantiate(bullet, shootPoint_crh.position, transform.rotation);
            if (!isFacingPositive)
            {
                GameObject bulletInstance = Instantiate(bullet, shootPoint_crh.position, transform.rotation);
                bulletInstance.transform.localScale = new Vector3(-0.007190318f, 0.008811175f, 1);
            }


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
            stepsEffect.Stop();
            isCrouching = true;
            animator.SetBool("crouching", true);
            speed *= 0.1f;

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

        if (context.performed && isTouchingBlocks)
        {
            animator.SetBool("climbing", true);
        }else
            animator.SetBool("climbing", false);
    }
    
    private void climbOver()
    {
        transform.position = climbOverPosition;
    }

    //player moves
    public void Move(InputAction.CallbackContext context)
    {
        stepsEffect.Play();
        horizontal = context.ReadValue<Vector2>().x;
        animator.SetFloat("running", Mathf.Abs(horizontal));

        if (context.canceled)
        {
            stepsEffect.Stop();
        }
        
    }

    public void physicsCheck()
    {
        //Ray
        Vector2 raycastDirection = isFacingPositive ? transform.right : -transform.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastLength, LayerMask.GetMask("Ground"));
        Color rayColor = Color.red;
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rayColor = Color.green;
            animator.SetBool("touchBlocks", true);
            isTouchingBlocks = true;
        }
        else
        {
            animator.SetBool("touchBlocks", false);
            isTouchingBlocks = false;
        }
        Debug.DrawRay(transform.position, raycastDirection * raycastLength, rayColor);
        Vector2 climbPosition = transform.position;
        climbBegunPosition = climbPosition + offset1;
        climbOverPosition = climbPosition + offset2;

        if (isTouchingBlocks)
        {
            transform.position = climbBegunPosition;
        }


        //Ground check
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
