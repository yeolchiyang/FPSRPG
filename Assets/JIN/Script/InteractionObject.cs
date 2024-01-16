using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    

    [SerializeField] int dropItemPattern;    // 0:h+h  1:h+d 
    [SerializeField] PortionCount[] portionCounts;
    string hillItemName = "Hill Portion";
    string debineItemName = "Debine Portion";

    Queue<string> itemNames = new Queue<string>();

    private void Start()
    {
        switch (dropItemPattern)
        {
            case 0:
                itemNames.Enqueue(hillItemName);
                itemNames.Enqueue(hillItemName);
                break;
            case 1:
                itemNames.Enqueue(hillItemName);
                itemNames.Enqueue(debineItemName);
                break;
        }
    }

    public string GetItemName() { return itemNames.Peek(); }
    public int GetItemCount() { return itemNames.Count;  }
    public void GetItem()
    {
        if (itemNames.Count > 0)
        {
            string firstItem = itemNames.Dequeue();
            if (firstItem == hillItemName)
                portionCounts[0].AddPotion(1);
            else if (firstItem == debineItemName)
                portionCounts[1].AddPotion(1);
        }
    }
}
