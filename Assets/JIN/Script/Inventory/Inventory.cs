using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    bool activeWindow = false;

    [SerializeField] public int hillPortionCount = 0;
    [SerializeField] public int debinePortionCount = 0;

    [SerializeField] GameObject ItemSlotPrefap;
    [SerializeField] GameObject SlotArea;
    [SerializeField] Player_Health player;

    Animator anim;
    List<ItemSlot> itemSlots = new List<ItemSlot>();

    

    int bookCount = 0;
    int dimensionOfPeice = 0;


    private void Awake()
    {
        inventory = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            hillPortionCount = PlayerPrefs.GetInt("player_hillPortion");
            debinePortionCount = PlayerPrefs.GetInt("player_debinePortion");
        }
        for(int i = 0; i < 4; ++i)
        {
            GameObject obj = Instantiate(ItemSlotPrefap, SlotArea.transform);
            ItemSlot itemSlot = obj.GetComponent<ItemSlot>();
            itemSlots.Add(itemSlot);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            activeWindow = !activeWindow;
            anim.SetBool("Appear", activeWindow);
        }

        //아이템 획득 조건을 상호작용으로 변경
        if (Input.GetKeyDown("9"))
        {
            if (bookCount < 4)
            {
                AddBookCount();
                insertItemToSlot(0, bookCount);
            }

            else
            {
                clearSlot(0);
                bookCount = 0;
            }

            
        }
        //여기도
        if (Input.GetKeyDown("o"))
        {
            if (dimensionOfPeice != 1)
            {
                dimensionOfPeice = 1;
                insertItemToSlot(1, dimensionOfPeice);
            }

            else
            {
                clearSlot(1);
                dimensionOfPeice = 0;
            }
        }
    }

    public void insertItemToSlot(int itemIndex, int itemCount)
    {
        for(int i = 0;i < itemSlots.Count; ++i)
        {
            if (itemIndex == itemSlots[i].getItemIndex())
            {
                itemSlots[i].SetSlotInfo(itemIndex, itemCount);
                return;
            }
        }

        for(int i = 0; i < itemSlots.Count; ++i)
        {
            if(itemSlots[i].getItemIndex() == -1)
            {
                itemSlots[i].SetSlotInfo(itemIndex, itemCount);
                return;
            }
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

    public void clearSlot(int itemIndex)
    {
        for (int i = 0; i < itemSlots.Count; ++i)
        {
            if (itemIndex == itemSlots[i].getItemIndex())
            {
                itemSlots[i].SetSlotInfo(-1);
                return;
            }
        }
    }

    

    public void AddBookCount() { ++bookCount; }
    public int getHillPortionCount() { return hillPortionCount; }
    public int getDebinePortionCount() { return debinePortionCount; }
    //public int getStatus(int stateIndex) { return status[stateIndex]; }
    //public int getBookCount() { return bookCount; }
}