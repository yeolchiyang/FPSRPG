using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_animation;

public class Player_Health : MonoBehaviour
{
    Player_Anima anima;
    public float maxHp = 100f;
    public float currentHp;
    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Player_Anima>();
        currentHp = maxHp;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            anima.Player_Hit();
            TakeDamage(10f);
            
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            anima.Player_Die();
            Debug.Log("ÇÃ·¹ÀÌ »ç¸Á");
        }
    }
}
