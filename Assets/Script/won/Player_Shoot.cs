using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.PackageManager;
using UnityEngine;
using Player_animation;

public class Player_Shoot : MonoBehaviour
{
    //public GameObject Bulletstartpoint;
    public GameObject BulletEffect;
    public GameObject BulletStartEffect;
    public GameObject BulletStartPoint;
    Transform BulletSatEffect;
    public float ShootDamage = 10f;
    public GameObject Gun;
    PlayerCtrl shootcheck;

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
                Enemy_Health enemy = hitInfo.collider.GetComponent<Enemy_Health>();

                //Vector3 targetDirection = hitInfo.point - transform.position;

                //targetDirection.y = 0f;

                //Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                //Gun.transform.rotation = Quaternion.Slerp(Gun.transform.rotation, targetRotation, Time.deltaTime);

                if (enemy != null)
                    enemy.TakeDamage(ShootDamage);
                if (ps != null)
                {
                    ps.Play();
                }


            }
            shootcheck.shootingcheck();
        }
    }
}
