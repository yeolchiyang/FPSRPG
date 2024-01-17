using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_animation;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
public class Player_Health : MonoBehaviour
{
    GameObject invenObj;
    Status_Inventory inventory;
    Launcher launcher;
    Machine machine;
    VGSGUN VGSGUN;
    PlayerCtrl PlayerCtrl;
    Player_Anima anima;
    FollowCamera followCamera;
    Collider collider;
    [SerializeField] ContralConditionBar ccb;
    public float maxHp = 100f;
    public float currentHp;
    public float maxMp = 100f;
    public float currentMp;
    public float currentExp;
    public float maxExp = 100f;
    public int lv = 1;
    public int forceSoul = 0;
    public float WeaponDamage = 0f;
    public bool BossHunting = false;
    float[] cctime = new float[3];
    public GameObject[] cc = new GameObject[3];
    public bool lief = true;
    float dam =0f;
    void Awake()
    {
        invenObj = GameObject.Find("StatusArea");
        inventory = invenObj.GetComponent<Status_Inventory>();
        Debug.Log("초기화");
        collider = GetComponent<Collider>();
        followCamera = Camera.main.GetComponent<FollowCamera>();
        BossHunting = false;
        PlayerCtrl = GetComponent<PlayerCtrl>();
        anima = GetComponent<Player_Anima>();
        currentHp = maxHp;
        currentMp = maxMp;
        currentExp = 0f;
    }
    private void Start()
    {
        
        StartCoroutine(Playermp());
        StartCoroutine(PlayerCrowdControl());
        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            currentHp = PlayerPrefs.GetFloat("player_CurretHp");
            currentMp = PlayerPrefs.GetFloat("player_CurretMp");
            currentExp = PlayerPrefs.GetFloat("player_currentExp");
            lv = PlayerPrefs.GetInt("player_Lv");
            forceSoul = PlayerPrefs.GetInt("player_forceSoul");
        }
        Debug.Log(SceneManager.GetActiveScene().name+PlayerPrefs.GetFloat("player_CurretHp"));
    }


    private void Update()
    {
        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            lv++;
            forceSoul++;
        }

        dam += Time.deltaTime;
        if (currentHp <= 0)
        {
            Die();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy" && currentHp > 0 && dam>1)
        {
            dam = 0;
            anima.Player_Hit();
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        if ((currentHp - (damage - inventory.status[4])) > 0)
        {

            anima.Player_Hit();
            currentHp -= (damage-inventory.status[4]);
            ccb.UpdateHP();
        }
        currentHp -= (damage - inventory.status[4]);
        ccb.UpdateHP();
    }

    public void Die()
    {
        collider.gameObject.SetActive(false);
        anima.Player_Die();
        Debug.Log("플레이어 사망");
        PlayerCtrl.ccon();
        lief = false;
    }

    public void AddExp(float getExp)
    {
        currentExp += getExp;
        ccb.UpdateEXP();
    }

    public void BossKill()
    {
        BossHunting = true;
    }

    public bool burn = false;
    public bool addicted = false;
    public bool restraint = false;

    public void PlayerCcoff()
    {
        burn = false;
        addicted = false;
        restraint = false;
        PlayerCtrl.ccoff();
    }
    public void PlayerCcon(int order)
    {
        switch (order)
        {
            case 0:
                burn = true;
                cctime[order] = 5;
                break;
            case 1:
                addicted = true;
                cctime[order] = 5;
                break;
            case 2:
                restraint = true;
                cctime[order] = 5;
                break;
            case 3:
                PlayerCtrl.bosscc();
                followCamera.CrowdControlcam();
                break;
            default:
                break;
        }
    }
    float maxspeet = 0;
    IEnumerator PlayerCrowdControl()
    {
        while (true)
        {
            CrowdControl();
            cc[0].SetActive(burn);
            cc[1].SetActive(addicted);
            cc[2].SetActive(restraint);
            yield return new WaitForSeconds(1f);
        }

    }
    IEnumerator Playermp()
    {
        while (true)
        {
            mp();
            yield return new WaitForSeconds(0.2f);
        }
    }
    void mp()
    {
        if (currentMp < maxMp)
        {
            currentMp += 1.6f;
        }
        else currentMp = maxMp;
    }
    public void CostMp(float costmp)
    {
        currentMp -= costmp;
    }
    void CrowdControl()
    {
        if (burn && cctime[0]>=0)
        {
            anima.Player_Hit();
            cctime[0]--;
            currentHp -= 5;
        }
        else 
        {
            burn = false;
        }
        if (addicted && cctime[1] >= 0)
        {
            anima.Player_Hit();
            cctime[1]--;
            if (currentHp > 5)
            {
                currentHp -= 5;
            }
        }
        else { addicted = false; }
        if (restraint && cctime[2] >= 0)
        {
            cctime[2]--;
            PlayerCtrl.speed = (maxspeet + inventory.status[3])/2;
            Debug.Log("이속감소" + PlayerCtrl.speed);
        }
        
        else if (!restraint || cctime[2] <0)
        {
            if (PlayerCtrl.speed > maxspeet)
            {
                maxspeet = PlayerCtrl.speed;
                Debug.Log("이속저장" + maxspeet);
            }
            PlayerCtrl.speed = maxspeet;
            restraint = false;
        }
    }

    public void SAVE()
    {
        Debug.Log("ASD");
        PlayerPrefs.GetFloat("player_CurretHp",currentHp);
        PlayerPrefs.SetFloat("player_CurretHp", currentHp);
    }
}
