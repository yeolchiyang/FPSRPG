using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;

    [SerializeField] int hillPortionCount = 0;
    [SerializeField] int debinePortionCount = 0;
    [SerializeField] QuestUI questUI;

    int bookCount = 0;

    private void Awake()
    {
        inventory = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown("9")) 
        {
            AddBookCount();
            print(bookCount);
            questUI.Create_QSlot(0);
        }
    }


    public void setPortionInfo(int portionIndex, int count)
    {
        switch (portionIndex)
        {
            case 0:
                hillPortionCount = count; break;
            case 1:
                debinePortionCount = count; break;
            default: 
                break;
        }
    }

    public void AddBookCount() { ++bookCount; }


    public int getHillPortionCount() { return hillPortionCount; }

    public int getDebinePortionCount() { return debinePortionCount; }

    public int getBookCount() { return bookCount; }
}