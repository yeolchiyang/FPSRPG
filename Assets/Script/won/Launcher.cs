using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Yang;

public class Launcher : MonoBehaviour
{
    public GameObject asd;
    BasicAim basicAim;
    WeaponChange WeaponChange;
    public GameObject BulletEffect;
    public GameObject BulletStartPoint;
    GameObject BulletEndPoint;
    private Camera Cam;
    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;
    public float MaxLength;
    private float Range = 100f;
    bool ok = true;

    // Start is called before the first frame update
    void Start()
    {
        basicAim = asd.GetComponent<BasicAim>();
    }
    float shootspeed = 0;
    // Update is called once per frame
    void Update()
    {
        BulletEndPoint = GameObject.Find("BulletEnd");
        shootspeed = shootspeed+Time.deltaTime;
        if (Input.GetMouseButton(0) && shootspeed >= 0.15&&ok)
        {
            shootspeed = 0;
            basicAim.ShootToAim();
            GameObject  ASD = Instantiate(BulletEffect, BulletStartPoint.transform.position, BulletStartPoint.transform.rotation);
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit(); // hit 오브젝트
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

            if (Physics.Raycast(ray, out hitInfo, Range, layerMask))
            {
                ASD.transform.LookAt(hitInfo.point);
                
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
    public void qwe()
    {
        ok = false;
    }
}
