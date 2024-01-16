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
    private float NextHitable = 0.1f;//�ߺ������� ���� �ð�
    private float HittedTime = 0f; //�����ð�
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
            //�¾Ҵ� �ð� + �ߺ������� ���� �ð� <= ����ð�
            if ((HittedTime + NextHitable) <= Time.time)
            {
                Debug.Log("aaa");
                skeleton = collision.gameObject.GetComponent<Skeleton>();
                skeleton.SetDamaged(weaponDamage);
                HittedTime = Time.time;//���� �ð� ��Ͽ�
            }
            
        }
        
        
    }
    public void dam(float dam)
    {
        weaponDamage = dam;
    }
}
