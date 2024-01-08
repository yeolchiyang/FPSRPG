using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField]GameObject QuestSlotPrefap;
    List<QuestSlot> questSlots = new List<QuestSlot>();

    public void Create_QSlot(int Q_Item_index)
    {
        //for(int i = 0; i < questSlots.Count; i++)
        //{
        //     = questSlots[i]
        //}

        GameObject Q_SlotObj = Instantiate(QuestSlotPrefap, transform);
        QuestSlot questSlot = Q_SlotObj.GetComponent<QuestSlot>();
        questSlot.SetQuestSlot(Q_Item_index);
        questSlots.Add(questSlot);
    }


}
