using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

   

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {

            Debug.Log("¸÷ »ç¸Á");
        }
    }
}
