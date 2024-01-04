using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.PackageManager;
using UnityEngine;
using Player_animation;
using Yang;

public class Player_Shoot : MonoBehaviour
{
    //public GameObject Bulletstartpoint;
    public GameObject BulletEffect;
    public GameObject BulletStartEffect;
    public GameObject BulletStartPoint;
    Transform BulletSatEffect;
    public float ShootDamage = 10f;

    private void Start()
    {
         BulletSatEffect = BulletStartPoint.GetComponent<Transform>();
    }
    float Range = 100f;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 startShootPosition = BulletStartPoint.transform.position;


            GameObject ShootEffect = Instantiate(BulletStartEffect, startShootPosition, Quaternion.identity, BulletStartPoint.transform);

            ParticleSystem psshoot = ShootEffect.GetComponent<ParticleSystem>();
            if (psshoot != null)
            {
                psshoot.Play();
            }

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();//hit 오브젝트
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

            if (Physics.Raycast(ray, out hitInfo,Range, layerMask))
            {
                GameObject bulletEffect = Instantiate(BulletEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                
                ParticleSystem ps = bulletEffect.GetComponent<ParticleSystem>();
                Skeleton enemy = hitInfo.collider.GetComponent<Skeleton>();
                if (enemy != null)
                    enemy.SetDamaged(ShootDamage);
                if (ps != null)
                {
                    ps.Play();
                }


            }
        }
    }
}
