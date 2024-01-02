using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContralConditionBar : MonoBehaviour
{
    [SerializeField] GameObject[] conditionBars;  // index 0:HP , 1:MP, 2:EXP
    //[SerializeField] GameObject Player;

    //�Ͻ� �Ķ����
    [SerializeField] float currentHP = 100;
    [SerializeField] float currentMP = 100;
    [SerializeField] float currentEXP = 10;
    float maxHP = 100;
    float maxMP = 100;
    float maxEXP = 100;

    private void Start()
    {
        //join�� �ּ����� �ӽ� �Ķ���� ����
        //player = Player.GetComponent<Player>();
        Fillindex(0, /*player.*/currentHP, /*player.*/maxHP);
        Fillindex(1, /*player.*/currentMP, /*player.*/maxMP);
        Fillindex(2, /*player.*/currentEXP, /*player.*/maxEXP);
    }

    private void Update()
    {
        //�ð��� Ȯ���� ���ؼ� ����, ������ ������ �����ϸ� �ش� ���� ����
        Fillindex(0, /*player.*/currentHP, /*player.*/maxHP);
        Fillindex(1, /*player.*/currentMP, /*player.*/maxMP);
        Fillindex(2, /*player.*/currentEXP, /*player.*/maxEXP);
    }

    void Fillindex(int index, float current, float max)
    {
        if (current >= 0)
            conditionBars[index].GetComponent<Image>().fillAmount =
                current / max;
    }

    public void TakeDamage(float damage)
    {
        //�÷��̾� Ŭ���� currentHP�� �������̶� ����
        /*player.*/currentHP -= damage;
        Fillindex(0, /*player.*/currentHP, /*player.*/maxHP);
    }

    public void CostMana(float costMana) 
    {
        /*player.*/currentMP -= costMana;
        Fillindex(1, /*player.*/currentMP, /*player.*/maxMP);
    }

    public void AddEXP(float exp)
    {
        /*player.*/currentEXP += exp;
        Fillindex(2, /*player.*/currentEXP, /*player.*/maxEXP);
    }
}