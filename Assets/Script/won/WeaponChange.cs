using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    Transform tr;
    public GameObject[] Weapon;
    int currentWeaponNumber = 0;
    public float WeaponDamage;
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
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponNumber = 1;
            WeaponON();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeaponNumber = 2;
            WeaponON();
        }
    }
    void WeaponON()
    {
        for (int i = 0; i < Weapon.Length; i++)
        {
            Weapon[i].SetActive(false);
        }
        Weapon[currentWeaponNumber].SetActive(true);
        WeaponDamage = currentWeaponNumber * 5;
    }
}
