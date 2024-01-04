using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class Launcher : MonoBehaviour
{
    //public GameObject Bulletstartpoint;
    public GameObject BulletEffect;
    public GameObject BulletStartEffect;
    public GameObject BulletStartPoint;


    private void Start()
    {
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

            if (Physics.Raycast(ray, out hitInfo, Range, layerMask))
            {
                GameObject bulletEffect = Instantiate(BulletEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                ParticleSystem ps = bulletEffect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
            }
        }
    }
}
