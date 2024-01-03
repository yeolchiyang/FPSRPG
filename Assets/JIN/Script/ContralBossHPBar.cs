using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContralBossHPBar : MonoBehaviour
{
    [SerializeField] GameObject BossHPBar;
    [SerializeField] UnityEngine.UI.Text HPBarText;
    [SerializeField] UnityEngine.UI.Text BossName;
    [SerializeField] float currentHP = 100;
    float maxHP = 100;
    string bossName = "DemonKing Diablo";

    private void Start()
    {
        gameObject.SetActive(false);
        BossName.text = bossName;
    }

    private void Update()
    {
        Fill(currentHP, maxHP);
        if(currentHP  <= 0 )
            gameObject.SetActive(false);
    }

    void Fill(float current, float max)
    {
        if (current >= 0)
        {
            BossHPBar.GetComponent<Image>().fillAmount =
                current / max;
            HPBarText.text = current + "/" + max;
        }
    }

    public void TakeDamage(float damage)
    {
        //�÷��̾� Ŭ���� currentHP�� �������̶� ����
        /*Boss.*/currentHP -= damage;
        Fill(/*Boss.*/currentHP, /*Boss.*/maxHP);
    }

    public void setBossInfo(float currentHP, float maxHP, string bossName)
    {
        this.currentHP = currentHP;
        this.maxHP = maxHP;
        this.bossName = bossName;
    }
}
