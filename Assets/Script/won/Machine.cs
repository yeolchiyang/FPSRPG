using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class Machine : MonoBehaviour
{
    GameObject invenObj;
    Status_Inventory inventory;
    GameObject player;
    Player_Health player_Health;
    public GameObject BulletEffect;
    public GameObject BulletStartEffect;
    public GameObject BulletStartPoint;
    Transform BulletSatEffect;
    float ShootDamage;
    bool ok = true;

    private void Start()
    {
        invenObj = GameObject.Find("StatusArea");
        inventory = invenObj.GetComponent<Status_Inventory>();
        player = GameObject.FindWithTag("Player");
        BulletSatEffect = BulletStartPoint.GetComponent<Transform>();
    }

    float Range = 100f;

    private void Update()
    {
        ShootDamage = 8 + (inventory.status[0] * 0.5f);
        player_Health = player.GetComponent<Player_Health>();
        if (Input.GetMouseButtonDown(0)&&player_Health.lief)
        {
            Vector3 startShootPosition = BulletStartPoint.transform.position;

            GameObject ShootEffect = Instantiate(BulletStartEffect, startShootPosition, Quaternion.identity, BulletStartPoint.transform);

            ParticleSystem psshoot = ShootEffect.GetComponent<ParticleSystem>();
            if (psshoot != null)
            {
                psshoot.Play();
            }

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

            if (Physics.Raycast(ray, out hitInfo, Range, layerMask))
            {
                
                GameObject bulletEffect = Instantiate(BulletEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                ParticleSystem ps = bulletEffect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
                Skeleton enemy = hitInfo.collider.GetComponent<Skeleton>();
                if (enemy != null)
                    enemy.SetDamaged(ShootDamage);
                HYDRA hYDRA = hitInfo.collider.GetComponent<HYDRA>();
                if(hYDRA != null)
                {
                    hYDRA.SetDamaged(ShootDamage);
                }
            }
        }
    }
   
}
