using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private Transform shootPoint_std;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject[] ammo;

    private int ammoAmount;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 9; i++)
        {
            ammo[i].gameObject.SetActive(false);
        }

        ammoAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed && ammoAmount >= 0)
        {
            var spawnedBullet = Instantiate(bullet, shootPoint_std.position, shootPoint_std.rotation);
            spawnedBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 500f);
            ammoAmount--;
            ammo[ammoAmount].gameObject.SetActive(false);
        }
    }

    public void Reolad(InputAction.CallbackContext context)
    {
        ammoAmount = 9;
        for(int i = 0; i < 9; i++)
        {
            ammo[i].gameObject.SetActive(true);
        }
    }
}
