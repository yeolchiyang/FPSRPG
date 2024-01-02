using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PortionCount : MonoBehaviour
{
    [SerializeField] int portionIndex = 0;
    [SerializeField] int portionCount = 10;
    public UnityEngine.UI.Text CountText;

    private void Start()
    {
        CountPortion();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown("1"))
    //        AddPortion(1);
    //    if (Input.GetKeyDown("2"))
    //        ReducePortion(1);
    //}

    void CountPortion()
    {
        CountText.text = "x" + portionCount;
    }

    public void AddPotion(int data)
    {
        portionCount += data;
        CountPortion();
    }

    public void ReducePotion(int data)
    {
        if (portionCount > 0)
        {
            portionCount -= data;
            CountPortion();
            //포션 사용 사운드 첨부가능
        }
        else
        {
            Debug.Log("Potion is empty. You can use this potion");
            //사용 불가 사운드 첨부가능
        }
    }
}
