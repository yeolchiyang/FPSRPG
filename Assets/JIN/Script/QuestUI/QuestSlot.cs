using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image icon;
    [SerializeField] UnityEngine.UI.Text text;
    [SerializeField] Sprite[] icons;

    int questIndex;

    public void SetQuestSlot(int index)
    {
        questIndex = index;
        switch (questIndex)
        {
            case 0:
                text.text = "������ �� x" + Inventory.inventory.getBookCount();
                icon.sprite = icons[0];
                break;
            case 1:
                text.text = "������ ����";
                icon.sprite = icons[1];
                break;
        }
    }
}
