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
    GameObject BulletEndPoint;
     Camera Cam;
    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;
    public float MaxLength;
    bool ok =true;
    float shootspeed;
    
    // Start is called before the first frame update
    void Start()
    {
        shootspeed = 0;

    }
    float Range = 100f;
    // Update is called once per frame
    void Update()
    {
        BulletEndPoint = GameObject.Find("BulletEnd");
        shootspeed = shootspeed+Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && shootspeed >= 1 && ok)
        {
            
            shootspeed=0;
            GameObject obj= Instantiate(BulletEffect, BulletStartPoint.transform.position, BulletStartPoint.transform.rotation);
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();//hit 오브젝트
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
            if (Physics.Raycast(ray, out hitInfo, Range, layerMask))
            {
                
                obj.transform.LookAt(hitInfo.point);
               
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
    public void asd()
    {
        ok = false;
    }
}
