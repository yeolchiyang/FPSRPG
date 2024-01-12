using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status_Inventory : MonoBehaviour
{
    [SerializeField] GameObject PowerUpUI;
    [SerializeField] Slider[] statusSliders;
    [SerializeField] UnityEngine.UI.Text[] powerUpUITexts;
    [SerializeField] Player_Health player;

    int[] status = new int[5];   //index 1:W1, 2:W2, 3:W3, 4:Vitality 5:Armor
    int statusMaxValue = 8;

    private void Start()
    {
        for (int i = 0; i < statusSliders.Length; ++i)
        {
            statusSliders[i].maxValue = statusMaxValue;
            statusSliders[i].value = 0;
        }
        player.forceSoul = 12;  //시각 확인후 삭제        

        for(int i = 0;i < powerUpUITexts.Length; ++i) 
        {
            powerUpUITexts[i].text = "" + status[i];
            if (status[i] == 0)
                powerUpUITexts[i].text = null;
        }
    }

    public void AddStatPoint(int stateIndex)
    {
        if (player.forceSoul > 0)
        {
            if (status[stateIndex] < 8)
            {
                ++status[stateIndex];
                UpdateStatus(stateIndex);
                powerUpUITexts[stateIndex].text = status[stateIndex].ToString();
                player.forceSoul--;


            }
            else
                Debug.Log("Already this state is maxim ");
        }
        else
        {
            Debug.Log("ForceSoul is Empty");
            PowerUpUI.SetActive(false);
        }
    }


    void UpdateStatus(int stateIndex)
    {
        statusSliders[stateIndex].value = status[stateIndex];
    }
}
