using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Yang;
using static UnityEditor.FilePathAttribute;

public class VGSGUN : MonoBehaviour
{
    WeaponChange WeaponChange;
    public GameObject BulletEffect;
    public GameObject BulletStartPoint;
    public GameObject BulletEndPoint;
     Camera Cam;
    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;
    public float MaxLength;
    float ShootDamage;
    float shootspeed;
    
    // Start is called before the first frame update
    void Start()
    {
        shootspeed = 0;
        ShootDamage = WeaponChange.WeaponDamage;
    }
    float Range = 100f;
    // Update is called once per frame
    void Update()
    {
        shootspeed += shootspeed+Time.deltaTime;
        
        if (Input.GetMouseButtonDown(0)&&shootspeed>2)
        {
            GameObject obj= Instantiate(BulletEffect, BulletStartPoint.transform.position, BulletStartPoint.transform.rotation);
            shootspeed = 0;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();//hit 오브젝트
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
            if (Physics.Raycast(ray, out hitInfo, Range, layerMask))
            {
                
                obj.transform.LookAt(hitInfo.point);
                Skeleton enemy = hitInfo.collider.GetComponent<Skeleton>();
                if (enemy != null)
                    enemy.SetDamaged(hitInfo, ShootDamage);

            }
            else
            {
                obj.transform.LookAt(BulletEndPoint.transform.position);
            }


            if (Cam != null)
            {
                RaycastHit hit;
                var mousePos = Input.mousePosition;
                RayMouse = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                if (Physics.Raycast(RayMouse.origin, RayMouse.direction, out hit, MaxLength))
                {
                    RotateToMouseDirection(gameObject, hit.point);
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
}
