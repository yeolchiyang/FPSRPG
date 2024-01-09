using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yang;

public class WeaponChange : MonoBehaviour
{
    Transform tr;
    public GameObject[] Weapon;
    int currentWeaponNumber = 0;
    public float WeaponDamage = 10;
    Skeleton skeleton;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        WeaponON();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponNumber = 0;
            WeaponON();
            WeaponDamage = 50;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponNumber = 1;
            WeaponON();
            WeaponDamage = 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeaponNumber = 2;
            WeaponON();
            WeaponDamage = 2;
        }
    }
    void WeaponON()
    {
        for (int i = 0; i < Weapon.Length; i++)
        {
            Weapon[i].SetActive(false);
        }
        Weapon[currentWeaponNumber].SetActive(true);
    }
    public void hit()
    {
        skeleton=GetComponent<Skeleton>();
        skeleton.SetDamaged(WeaponDamage);
    }
}
