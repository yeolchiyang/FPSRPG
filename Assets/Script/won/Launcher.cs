using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Yang;

public class Launcher : MonoBehaviour
{
    public GameObject AIM;
    Player_Health player_Health;
    BasicAim basicAim;
    public GameObject player;
    public GameObject invenObj;
    Status_Inventory inventory;
    public float weaponDamage;
    public GameObject BulletEffect;
    public GameObject BulletStartPoint;
    public GameObject BulletEndPoint;
    private Camera Cam;
    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;
    public float MaxLength;
    private float Range = 100f;
    float costMp = 2;
    public float test = 0;
    GameObject bullet;
    shoothit GetShoothit;
    public AudioClip bulletsound;
    // Start is called before the first frame update
    void Start()
    {
        AIM = GameObject.Find("BImAIm");
        BulletEndPoint = GameObject.Find("BulletEnd");
        bullet = GameObject.FindWithTag("Bullet");
        GetShoothit = BulletEffect.GetComponent<shoothit>();
        invenObj = GameObject.Find("StatusArea");
        inventory = invenObj.GetComponent<Status_Inventory>();
        player = GameObject.FindWithTag("Player");
        player_Health = player.GetComponent<Player_Health>();
        basicAim = AIM.GetComponent<BasicAim>();
    }
    float shootspeed = 0;
    // Update is called once per frame
    void Update()
    {
        
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            player_Health = player.GetComponent<Player_Health>();
        }
        if (invenObj == null)
        {
            invenObj = GameObject.Find("StatusArea");
            inventory = invenObj.GetComponent<Status_Inventory>();
        }
        player_Health = player.GetComponent<Player_Health>();
        inventory = invenObj.GetComponent<Status_Inventory>();
        shootspeed = shootspeed+Time.deltaTime;
        if (Input.GetMouseButton(0) && shootspeed >= 0.3 - (inventory.status[2]* 0.025f)&& player_Health.lief && player_Health.currentMp > costMp)
        {
            AudioSource.PlayClipAtPoint(bulletsound, transform.position);
            player_Health.CostMp(costMp);
            shootspeed = 0;
            basicAim.ShootToAim();  // Weapon3 ø°¿” 
            GameObject  ASD = Instantiate(BulletEffect, BulletStartPoint.transform.position, BulletStartPoint.transform.rotation);
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

            if (Physics.Raycast(ray, out hitInfo, Range, layerMask))
            {
                
                ASD.transform.LookAt(hitInfo.point);


            }
            else
            {
                ASD.transform.LookAt(BulletEndPoint.transform.position);
                
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
        
    }

    void RotateToMouseDirection(GameObject obj, Vector3 destination)
    {
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
    
}
