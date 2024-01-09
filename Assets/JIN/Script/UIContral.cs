using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContral : MonoBehaviour
{
    int currentSetArmIndex;

    //GameObject Boss;
    //GameObject Player;
    
    [SerializeField] GameObject BossHPBar;
    [SerializeField] GameObject[] Aims;

    [SerializeField] GameObject MainMemu;
    bool mainMenuActivite = false;

    [SerializeField] UnityEngine.UI.Image[] Arms;
    
    [SerializeField] UnityEngine.UI.Text PlayerLv;
    [SerializeField] UnityEngine.UI.Text ForceSoulCount;

    List<GameObject> aims = new List<GameObject>();

    

    private void Start()
    {
        //Player player = Player.GetComponent<Player>()
        //Boss boss = Boss.GetComponent<Boss>();

        MainMemu.SetActive(false);

        for (int i = 0; i < Aims.Length; ++i) {
            aims.Add(Aims[i]);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        PlayerLv.text = "" + /*player.lv*/1;
        ForceSoulCount.text = "" + /*player.forceSoul*/10;

        if (Input.GetKeyDown("1"))  //basic gun
        {
            if (aims[currentSetArmIndex] != null)
                resetAim();
            setAim(0);
        }

        if (Input.GetKeyDown("2"))  //aim2
        {
            if (aims[currentSetArmIndex] != null)
                resetAim();
            setAim(1);
        }

        if (Input.GetKeyDown("3"))  //aim3
        {
            if (aims[currentSetArmIndex] != null)
                resetAim();
            setAim(2);
        }

        if (Input.GetKeyDown("0"))  //보스체력바 활성화
        {
            if (!BossHPBar.activeSelf)
            {
                BossHPBar.SetActive(true);
                ContralBossHPBar bossHPBarSample =
                    BossHPBar.GetComponent<ContralBossHPBar>();
                bossHPBarSample.setBossInfo(
                    /*boss.currentHP*/100, /*boss.maxHP*/100, /*boss.name*/"DemonKing Diablo");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 1)
                Time.timeScale = 0;
            else if (Time.timeScale == 0)
                Time.timeScale = 1;

            mainMenuActivite = !mainMenuActivite;
            MainMemu.SetActive(mainMenuActivite);
        }
    }

    void setAim(int armIndex)
    {
        if (!aims[armIndex].activeSelf)
        {
            aims[armIndex].SetActive(!aims[armIndex].activeSelf);
            currentSetArmIndex = armIndex;
            Arms[armIndex].color = Color.red;
        }
    }

    void resetAim()
    {
        aims[currentSetArmIndex].SetActive(false);
        Arms[currentSetArmIndex ].color = new Color(0.17f, 0.17f, 0.17f);
    }
}
