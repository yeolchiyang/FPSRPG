using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class VGSGUN : MonoBehaviour
{
    public GameObject BulletEffect;
    public GameObject BulletStartPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float Range = 100f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            GameObject ShootEffect = Instantiate(BulletEffect, BulletStartPoint.transform.position, Quaternion.identity, BulletStartPoint.transform);
            
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
               


            }
        }
    }
}
