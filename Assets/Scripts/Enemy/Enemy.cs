using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemy_rb;
    private bool isFacingNegative = true;
    public float enemy_speed;
    public float enemy_radius;
    public Animator animator;

    [Header("Walk range")]
    public Transform enemy_leftPoint;
    public Transform enemy_rightPoint;
    private float enemy_leftBorder, enemy_rightBorder;

    [Header("Take damage")]
    public int enemy_health = 100;
    public int damage;

    [Header("Alert area")]
    [SerializeField] float enemy_raycastLength;
    [SerializeField] Vector2 offset;
    private Vector2 eyePosition;
    public bool isAlerted;

    [Header("Attack")]
    public bool enemy_canAttack;
    public float distanceToPlayer;


    void Start()
    {
        enemy_rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();

        enemy_leftBorder = enemy_leftPoint.position.x;
        enemy_rightBorder = enemy_rightPoint.position.x;
        Destroy(enemy_leftPoint.gameObject);
        Destroy(enemy_rightPoint.gameObject);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("02Attack") && stateInfo.normalizedTime >= 0.5f)
        {
            SetActive();
        }
    }

    void Update()
    {
        alertArea();
        if(isAlerted)
            chase();
        else
            enemy_move();

        enemy_attack();

    }

    private void enemy_move()
    {

        if (isFacingNegative)
        {
            enemy_rb.velocity = new Vector2(-enemy_speed, enemy_rb.velocity.y);
            if (transform.position.x < enemy_leftBorder)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isFacingNegative = false;
            }
        }
        else
        {
            enemy_rb.velocity = new Vector2(enemy_speed, enemy_rb.velocity.y);
            if (transform.position.x > enemy_rightBorder)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isFacingNegative = true;
            }
        }
        enemy_animation();
    }


    public void enemy_takeDamage(int damage)
    {
        enemy_health -= damage;
        if (enemy_health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Killed!");
        }
    }

    private void alertArea()
    {
        Vector2 raycastDirection = isFacingNegative ? -transform.right : transform.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, enemy_raycastLength, LayerMask.GetMask("Character"));
        Color rayColor = Color.green;
        isAlerted = false;
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            rayColor = Color.red;
            isAlerted = true;
        }

        Vector2 enemy_Position = transform.position;
        eyePosition = enemy_Position + offset;

        Debug.DrawRay(eyePosition, raycastDirection * enemy_raycastLength, rayColor);
    }

    private void chase()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPosition = player.transform.position;
        Rigidbody2D player_rb = player.GetComponent<Rigidbody2D>();

        Vector2 direction = playerPosition - (Vector2)transform.position;
        direction.Normalize();
               
        if ((Vector2.Distance(transform.position, playerPosition)) < distanceToPlayer)
        {
            enemy_rb.velocity = Vector2.zero;
            enemy_canAttack = true;
        }
        else
        {
            enemy_canAttack = false;
            enemy_rb.velocity = direction * enemy_speed * 1.5f;
            if (direction.x < 0 && !isFacingNegative)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isFacingNegative = true;
            }
            else if (direction.x > 0 && isFacingNegative)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isFacingNegative = false;
            }
        }
        enemy_animation();
    }

    public void enemy_attack()
    {
        if (enemy_canAttack)
        {
            animator.SetBool("attacking", true);
            Debug.Log("Enemy attacked!");
        }
        else
            animator.SetBool("attacking", false);
    }

    public void enemy_animation()
    {
        if (enemy_rb.velocity != Vector2.zero)
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    public void SetActive()
    {
        GameObject hitBox = GameObject.Find("Enemies/hitBox");
        if (hitBox != null && !hitBox.activeSelf)
        {
            Debug.Log("called");
            hitBox.SetActive(true);
        }
    }

    public void new_SetActive()
    {
        GameObject hitBox = GameObject.Find("Enemies/hitBox");
        if (hitBox != null && !hitBox.activeSelf)
        {
            hitBox.SetActive(false);
        }
    }
}