using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionContral : MonoBehaviour
{
    [SerializeField] GameObject[] Portions;
    List<PortionCount> PortionList = new List<PortionCount> ();

    private void Start()
    {
        for (int i = 0; i < Portions.Length; ++i)
        {
            PortionCount PC = Portions[i].GetComponent<PortionCount>();
            PortionList.Add(PC);    //index 0:체력  1:상태이상
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
            PortionList[0].AddPotion(1);
        if (Input.GetKeyDown("2"))
            PortionList[0].ReducePotion(1);
        if (Input.GetKeyDown("3"))
            PortionList[1].AddPotion(1);
        if (Input.GetKeyDown("4"))
            PortionList[1].ReducePotion(1);
    }
}
