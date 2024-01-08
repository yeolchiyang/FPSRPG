using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionContral : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject[] Portions;
    [SerializeField] GameObject DebineEff;
    [SerializeField] GameObject HillEff;
    List<PortionCount> PortionList = new List<PortionCount> ();

    float portionDilay = 2f;
    float countTime = 0;

    private void Start()
    {
        
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
        if (Input.GetKeyDown("6") && countTime <= 0)
        {
            HillEff.SetActive(!PortionList[0].isEmpty());
            PortionList[0].ReducePotion(1);
            countTime = portionDilay;
        }
        if (Input.GetKeyDown("7"))
            PortionList[1].AddPotion(1);
        if (Input.GetKeyDown("8") && countTime <= 0)
        {
            DebineEff.SetActive(!PortionList[1].isEmpty());
            PortionList[1].ReducePotion(1);
            countTime = portionDilay;
        }

        countTime -= Time.deltaTime;
    }

    void DebineEffect()
    {
        DebineEff.SetActive(true);
    }
}
