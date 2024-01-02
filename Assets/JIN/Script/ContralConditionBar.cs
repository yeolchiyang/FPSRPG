using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContralConditionBar : MonoBehaviour
{
    [SerializeField] GameObject[] conditionBars;  // index 0:HP , 1:MP, 2:EXP
    //[SerializeField] GameObject Player;

    //일시 파라미터
    [SerializeField] float currentHP = 100;
    [SerializeField] float currentMP = 100;
    [SerializeField] float currentEXP = 10;
    float maxHP = 100;
    float maxMP = 100;
    float maxEXP = 100;

    private void Start()
    {
        //join후 주석해제 임시 파라미터 제거
        //player = Player.GetComponent<Player>();
        Fillindex(0, /*player.*/currentHP, /*player.*/maxHP);
        Fillindex(1, /*player.*/currentMP, /*player.*/maxMP);
        Fillindex(2, /*player.*/currentEXP, /*player.*/maxEXP);
    }

    private void Update()
    {
        //시각적 확인을 위해서 존재, 데미지 연산이 가능하면 해당 문장 삭제
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
        //플레이어 클래스 currentHP를 가질것이라 가정
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