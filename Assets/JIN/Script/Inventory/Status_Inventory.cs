using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status_Inventory : MonoBehaviour
{
    [SerializeField] Slider[] statusSliders;
    [SerializeField] GameObject Player;
    Player_Health player;

    int statusMaxValue = 8;

    private void Awake()
    {
        //player = Player.GetComponent<Player_Health>();
    }

    private void Start()
    {
        for (int i = 0; i < statusSliders.Length; ++i)
        {
            statusSliders[i].maxValue = statusMaxValue;
            statusSliders[i].value = 0;
        }

        ChangeStatus(0);
        ChangeStatus(1);
        ChangeStatus(2);
        ChangeStatus(3);
        ChangeStatus(4);
    }


    void ChangeStatus(int stateIndex)
    {
        switch (stateIndex)
        {
            case 0:
                statusSliders[stateIndex].value = 3/*player.W1_ATK*/; break; //getATK()
            case 1:
                statusSliders[stateIndex].value = 1/*player.W2_ATK*/; break; //getATK()
            case 2:
                statusSliders[stateIndex].value = 0/*player.W3_ATK*/; break; //getATK()
            case 3:
                statusSliders[stateIndex].value = 2/*player.Vitality*/; break; //getATK()
            case 4:
                statusSliders[stateIndex].value = 2/*player.Armor*/; break; //getATK()
        }
    }
}
