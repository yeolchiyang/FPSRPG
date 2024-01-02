using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionContral : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject[] Portions;
    [SerializeField] GameObject[] PortionEffects;
    List<PortionCount> PortionList = new List<PortionCount> ();
    List<GameObject> PortionEffectList = new List<GameObject>();

    private void Start()
    {
        
        for (int i = 0; i < Portions.Length; ++i)
        {
            PortionCount PC = Portions[i].GetComponent<PortionCount>();
            PortionList.Add(PC);    //index 0:체력  1:상태이상
        }
        for (int i = 0; i < PortionEffects.Length; ++i)
        {
            GameObject obj = PortionEffects[i];
            obj.SetActive(false);
            PortionEffectList.Add(obj);    //index 0:회복이펙트  1:상태이상 회복이펙트
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
        {
            PortionList[1].ReducePotion(1);
            //DisplayEffect(PortionEffectList[1]);
        }
    }

    void DisplayEffect(GameObject obj)
    {
        obj.SetActive(true);
        anim = obj.GetComponent<Animator>();
        anim.SetTrigger("Debine");
    }
}
