using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Yang;
using static UnityEditor.FilePathAttribute;

public class VGSGUN : MonoBehaviour
{
    GameObject invenObj;
    Status_Inventory inventory;
    public float weaponDamage;
    Player_Health player_Health;
    public GameObject player;
    public GameObject BulletEffect;
    public GameObject BulletStartPoint;
    GameObject BulletEndPoint;
    Camera Cam;
    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;
    public float MaxLength;
    float shootspeed;
    float costMp = 40;
    GameObject bullet;
    shoothit GetShoothit;
    
    // Start is called before the first frame update
    void Start()
    {
        BulletEndPoint = GameObject.Find("BulletEndPoint");
        bullet = GameObject.FindWithTag("Bullet");
        GetShoothit = BulletEffect.GetComponent<shoothit>();
        shootspeed = 0;
        player_Health = player.GetComponent<Player_Health>();
    }
    float Range = 100f;
    float curretMp;
    // Update is called once per frame
    void Update()
    {
        player_Health= player.GetComponent<Player_Health>();
        curretMp = player_Health.currentMp; 
        
        BulletEndPoint = GameObject.Find("BulletEndPoint");
        shootspeed = shootspeed+Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && shootspeed >= 1 && player_Health.lief && curretMp > costMp)
        {
            player_Health.CostMp(costMp);
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
    
}
