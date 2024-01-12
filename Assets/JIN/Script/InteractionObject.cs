using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [SerializeField] string itemName;

    public string getItemName() { return itemName; }
}
