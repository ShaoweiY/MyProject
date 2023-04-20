using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemy_rb;
    private bool isFacingNegative = true;
    public float enemy_speed;
    public float enemy_radius;

    //enemy walks range
    public Transform enemy_leftPoint, enemy_rightPoint;
    private float enemy_leftBorder, enemy_rightBorder;

    //take damage
    public int enemy_health = 100;
    public int damage;

    //ray
    [SerializeField] float enemy_raycastLength;
    [SerializeField] Vector2 offset;
    private Vector2 eyePosition;
    public bool isAlerted;

    void Start()
    {
        enemy_rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();

        enemy_leftBorder = enemy_leftPoint.position.x;
        enemy_rightBorder = enemy_rightPoint.position.x;
        Destroy(enemy_leftPoint.gameObject);
        Destroy(enemy_rightPoint.gameObject);
    }

    void Update()
    {
        playerCheck();
        if(isAlerted)
            chase();
        else
            enemy_move();
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

    private void playerCheck()
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
        // Get the player's position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPosition = player.transform.position;

        // Move towards the player
        Vector2 direction = playerPosition - (Vector2)transform.position;
        direction.Normalize();
        enemy_rb.velocity = direction * enemy_speed;

        // Flip the sprite if needed
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
}