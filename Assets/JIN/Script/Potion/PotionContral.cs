using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionContral : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject[] Portions;
    List<PortionCount> PortionList = new List<PortionCount>();

    
    [SerializeField] GameObject DebineEff;
    [SerializeField] GameObject HillEff;
    float hillPower = 20f;

    [SerializeField] ContralConditionBar ccb;

    [SerializeField] GameObject Player;
    Player_Health player;

    float portionDilay = 2f;
    float countTime = 0;

    private void Start()
    {
        player = Player.GetComponent<Player_Health>();
        
        for (int i = 0; i < Portions.Length; ++i)
        {
            PortionCount PC = Portions[i].GetComponent<PortionCount>();
            PortionList.Add(PC);    //index 0:체력  1:상태이상
        }
        DebineEff.SetActive (false);
        HillEff.SetActive (false);
    }

    

    private void Update()
    {
        if (Input.GetKeyDown("5"))
            PortionList[0].AddPotion(1);
        if (Input.GetKeyDown("e") && countTime <= 0)
        {
            HillEff.SetActive(!PortionList[0].isEmpty());
            PortionList[0].ReducePotion(1);
            if (player.currentHp + hillPower > 100)
            {
                player.currentHp = 100;
                ccb.UpdateHP();
            }
            else
            {
                player.currentHp += hillPower;
                ccb.UpdateHP();
            }

            countTime = portionDilay;
        }
        if (Input.GetKeyDown("7"))
            PortionList[1].AddPotion(1);
        if (Input.GetKeyDown("r") && countTime <= 0)
        {
            DebineEff.SetActive(!PortionList[1].isEmpty());
            PortionList[1].ReducePotion(1);
            countTime = portionDilay;
        }

        countTime -= Time.deltaTime;
    }
}
