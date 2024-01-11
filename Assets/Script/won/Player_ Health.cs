using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_animation;

public class Player_Health : MonoBehaviour
{
    Launcher launcher;
    Machine machine;
    VGSGUN VGSGUN;
    PlayerCtrl PlayerCtrl;
    Player_Anima anima;
    public float maxHp = 100f;
    public float currentHp;
    public float maxMp = 100f;
    public float currentMp;
    public float currentExp;
    public float maxExp = 100f;
    public float lv = 1f;
    public float forceSoul = 0f;
    public float WeaponDamage = 0f;
    public bool BossHunting = false;
    // Start is called before the first frame update
    void Start()
    {
        BossHunting = false;
        PlayerCtrl = GetComponent<PlayerCtrl>();
        anima = GetComponent<Player_Anima>();
        currentHp = maxHp;
        currentMp = maxMp;
        currentExp = 0f;
    }

    private void Update()
    {   
        if(currentExp >= maxExp)
        {
            currentExp -= maxExp;
            lv++;
            forceSoul++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy" && currentHp>0)
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
            die();
        }
    }
    public void die()
    {
        anima.Player_Die();
        Debug.Log("ÇÃ·¹ÀÌ »ç¸Á");
        PlayerCtrl.sss();
        launcher.qwe();
        machine.zxc();
        VGSGUN.asd();
    }
    public void ADDExp()
    {
        currentExp += 10;
    }
    public void BossKill()
    {
        BossHunting = true;
    }
    public void asd()
    {
        Debug.Log("qwe");
    }
}
