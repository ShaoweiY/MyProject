using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float bulletExistTime;
    public int bulletDamage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, bulletExistTime);
    }

    void FixedUpdate()
    {
        transform.Translate(transform.right * transform.localScale.x * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.enemy_takeDamage(bulletDamage);
                Debug.Log(enemy.enemy_health);
            }
            Destroy(gameObject);

        }

        if (other.gameObject.tag == "Blocks")
        {
            Destroy(gameObject);
            Debug.Log("Hit the box!");

        }
    }
}
