using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBox : MonoBehaviour
{
    public Transform enemy;

    void Update()
    {
        transform.position = new Vector3(enemy.position.x, enemy.position.y, 0);
        if (enemy.localScale.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }


    public void SetActive()
    {
        GameObject.Find("Enemies/Enemy/hitBox").SetActive(true);
    }

    public void new_SetActive()
    {
        GameObject.Find("Enemies/Enemy/hitBox").SetActive(false);
    }
}
