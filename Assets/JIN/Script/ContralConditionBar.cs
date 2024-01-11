using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContralConditionBar : MonoBehaviour
{
    [SerializeField] GameObject[] conditionBars;  // index 0:HP , 1:MP, 2:EXP
    [SerializeField] GameObject Player;
    Player_Health player;

    //�Ͻ� �Ķ����
    //[SerializeField] float currentHP = 100;
    //[SerializeField] float currentMP = 100;
    //[SerializeField] float currentEXP = 10;
    //float maxHP = 100;
    //float maxMP = 100;
    //float maxEXP = 100;

    private void Start()
    {
        //join�� �ּ����� �ӽ� �Ķ���� ����
        player = Player.GetComponent<Player_Health>();

        Fillindex(0, player.currentHp, player.maxHp);
        Fillindex(1, player.currentMp, player.maxMp);
        Fillindex(2, player.currentExp, player.maxExp);
    }

    //private void Update()
    //{
    //    Fillindex(0, player.currentHp, player.maxHp);
    //    Fillindex(1, player.currentMp, player.maxMp);
    //    Fillindex(2, player.currentExp, player.maxExp);
    //}

    void Fillindex(int index, float current, float max)
    {
        if (current >= 0)
            conditionBars[index].GetComponent<Image>().fillAmount =
                current / max;
    }

    public void UpdateHP()
    {
        //�÷��̾� Ŭ���� currentHp�� �������̶� ����
        Fillindex(0, player.currentHp, player.maxHp);
    }

    public void CostMana(float costMana) 
    {
        ///*player.*/currentMP -= costMana;
        Fillindex(1, player.currentMp, player.maxMp);
    }

    public void UpdateEXP(/*float exp*/)
    {
        //player.currentExp += exp;
        Fillindex(2, player.currentExp, player.maxExp);
    }
}