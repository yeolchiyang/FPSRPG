using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContral : MonoBehaviour
{
    int currentSetArmIndex;

    //GameObject Boss;
    public GameObject Player;

    Player_Health player;
    
    [SerializeField] GameObject BossHPBar;
    [SerializeField] GameObject[] Aims;

    [SerializeField] UnityEngine.UI.Image[] Arms;
    
    [SerializeField] UnityEngine.UI.Text PlayerLv;
    [SerializeField] UnityEngine.UI.Text ForceSoulCount;

    List<GameObject> aims = new List<GameObject>();

    private void Start()
    {
        player = Player.GetComponent<Player_Health>();
        //Boss boss = Boss.GetComponent<Boss>();
        for (int i = 0; i < Aims.Length; ++i) {
            aims.Add(Aims[i]);
        }
    }

    void Update()
    {
        PlayerLv.text = "" + player.lv;
        ForceSoulCount.text = "" + player.forceSoul;

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
