using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image ItemImage;
    [SerializeField] UnityEngine.UI.Text ItemCount;

    [SerializeField] Sprite[] itemImages;

    int itemIndex = -1;
    int itemCount;

    public void SetSlotInfo(int itemIndex, int itemCount = 0)
    {
        
            this.itemIndex = itemIndex;
            this.itemCount = itemCount;


        if (itemIndex == -1)
        {
            ItemImage.sprite = null;
            ItemCount.text = "";
        }
        else
        {
            ItemImage.sprite = itemImages[itemIndex];
            ItemCount.text = "" + itemCount;
        }

    }

    public int getItemIndex() { return itemIndex; }
}
