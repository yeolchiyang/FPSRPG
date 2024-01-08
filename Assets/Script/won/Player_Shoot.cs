using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.PackageManager;
using UnityEngine;
using Player_animation;
using Yang;

public class Player_Shoot : MonoBehaviour
{

    WeaponChange weaponChange;
    public RaycastHit hitInfo;
    private void Start()
    {
        
    }
    float Range = 100f;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            hitInfo = new RaycastHit();//hit 오브젝트
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

            if (Physics.Raycast(ray, out hitInfo,Range, layerMask))
            {
               
                Skeleton enemy = hitInfo.collider.GetComponent<Skeleton>();
                if (enemy != null)
                    weaponChange.hit();
                
            }
        }
    }
}
