using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBox : MonoBehaviour
{

    public void SetActive()
    {
        GameObject.Find("Enemies/Enemy/hitBox").SetActive(true);
    }

    public void new_SetActive()
    {
        GameObject.Find("Enemies/Enemy/hitBox").SetActive(false);
    }
}
