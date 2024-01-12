using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_animation;
using TMPro.EditorUtilities;

public class Player_Health : MonoBehaviour
{
    Launcher launcher;
    Machine machine;
    VGSGUN VGSGUN;
    PlayerCtrl PlayerCtrl;
    Player_Anima anima;

    [SerializeField] ContralConditionBar ccb;

    public float maxHp = 100f;
    public float currentHp;
    public float maxMp = 100f;
    public float currentMp;
    public float currentExp;
    public float maxExp = 100f;
    public int lv = 1;         //Áø¼±À± float -> int
    public int forceSoul = 0;  //Áø¼±À± float -> int
    public float WeaponDamage = 0f;
    public bool BossHunting = false;
    // Start is called before the first frame update
    void Awake()    //Áø¼±À± start -> awake ÀüÈ¯
    {
        StartCoroutine(playerCrowdControl());
        BossHunting = false;
        PlayerCtrl = GetComponent<PlayerCtrl>();
        anima = GetComponent<Player_Anima>();
        currentHp = maxHp;
        currentMp = maxMp;
        currentExp = 0f;
    }
    public float s;
    private void Update()
    {
        
        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            lv++;
            forceSoul++;
        }
        s += Time.deltaTime;
       
        if (Input.GetKeyDown(KeyCode.Tab)&&s>0.2)
        {
            
            if (true)
            {
                playerccon(a);
                a++;
                s = 0;
            }

        }
    }
    public int a = 0;
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
        ccb.UpdateHP();  //Áø¼±À± Ãß°¡
        if (currentHp <= 0)
        {
            die();
        }
    }
    public void die()
    {
        anima.Player_Die();
        Debug.Log("ÇÃ·¹ÀÌ »ç¸Á");
        PlayerCtrl.ccon();
        launcher.qwe();
        machine.zxc();
        VGSGUN.asd();
    }
    public void ADDExp()
    {
        currentExp += 10;
        ccb.UpdateEXP(); //Áø¼±À±
    }
    public void BossKill()
    {
        BossHunting = true;
    }

    public bool burn = false;
    public bool addicted = false;
    public bool restraint = false;

    public void playerccoff()
    {
        burn = false;
        addicted = false;
        restraint = false;
        PlayerCtrl.ccoff();
    }

    public void playerccon(int order)
    {
        switch (order)
        {
            case 0:
                burn = true;
                break;
            case 1:
                addicted = true;
                break;
            case 2:
                restraint = true;
                break;
            case 3:
                PlayerCtrl.ccon();
            break;
            default: break;

        }
       
    }

    IEnumerator playerCrowdControl()
    {
        while (true)
        {
            if (burn)
            {
                Debug.Log("a");
                currentHp -= 5;
            }
            if (addicted)
            {
                Debug.Log("aa");
                if (currentHp > 0) 
                {
                    currentHp -= 5;
                }
            }
            if (restraint)
            {
                Debug.Log("aaa");
                PlayerCtrl.speed = 2;
            }
            else if (!restraint) 
            {
                PlayerCtrl.speed = 4;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    

}
