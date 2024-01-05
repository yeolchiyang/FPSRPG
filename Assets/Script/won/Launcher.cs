using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Yang;

public class Launcher : MonoBehaviour
{
    WeaponChange WeaponChange;
    public GameObject BulletEffect;
    public GameObject BulletStartPoint;
    public GameObject BulletEndPoint;
    private Camera Cam;
    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;
    public float MaxLength;
    private float ShootDamage;
    private float Range = 100f;

    // Start is called before the first frame update
    void Start()
    {
        ShootDamage = WeaponChange.WeaponDamage;
    }
    float shootspeed = 0;
    // Update is called once per frame
    void Update()
    {
        shootspeed = shootspeed+Time.deltaTime;
        if (Input.GetMouseButton(0) && shootspeed >= 0.15)
        {
            shootspeed = 0;
            Debug.Log(shootspeed);
            
            GameObject  ASD = Instantiate(BulletEffect, BulletStartPoint.transform.position, BulletStartPoint.transform.rotation);
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit(); // hit 오브젝트
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

            if (Physics.Raycast(ray, out hitInfo, Range, layerMask))
            {
                ASD.transform.LookAt(hitInfo.point);
                Skeleton enemy = hitInfo.collider.GetComponent<Skeleton>();
                if (enemy != null)
                    enemy.SetDamaged(hitInfo, ShootDamage);
            }
            else
            {
                ASD.transform.LookAt(BulletEndPoint.transform.position);
            }
        }
        
    }

    void RotateToMouseDirection(GameObject obj, Vector3 destination)
    {
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
    
}
