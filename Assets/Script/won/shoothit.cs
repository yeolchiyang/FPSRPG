using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class shoothit : MonoBehaviour
{
    GameObject invenObj;
    Status_Inventory inventory;
    Skeleton skeleton;
    public GameObject bullet;
    public float weaponDamage;
    [SerializeField] private LayerMask EnemyLayer;
    private float NextHitable = 0.1f;//중복데미지 방지 시간
    private float HittedTime = 0f; //맞은시간
    // Start is called before the first frame update
    void Start()
    {
        invenObj = GameObject.Find("StatusArea");
        inventory = invenObj.GetComponent<Status_Inventory>();
    }
    private void Update()
    {
        if (inventory != null) 
        {
            invenObj = GameObject.Find("StatusArea");
            inventory = invenObj.GetComponent<Status_Inventory>();
        }
        if (gameObject.name == bullet.name)
        {
            weaponDamage = 40 + (inventory.status[1] * 8.75f);
        }
        else if (gameObject.name != bullet.name)
        {
            weaponDamage = 2 + (inventory.status[2] * 0.75f);
        }
        Debug.Log(weaponDamage);
    }
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        HYDRA hYDRA = collision.gameObject.GetComponent<HYDRA>();
        if (hYDRA != null)
        {
            
            hYDRA.SetDamaged(weaponDamage);
        }
        else if (((1 << collision.gameObject.layer) & EnemyLayer.value) != 0)
        {
            //맞았던 시간 + 중복데미지 방지 시간 <= 현재시간
            if ((HittedTime + NextHitable) <= Time.time)
            {
                Debug.Log("aaa");
                skeleton = collision.gameObject.GetComponent<Skeleton>();
                skeleton.SetDamaged(weaponDamage);
                HittedTime = Time.time;//맞은 시간 기록용
            }
            
        }
        
        
    }
    public void dam(float dam)
    {
        weaponDamage = dam;
    }
}
