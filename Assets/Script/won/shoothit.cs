using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class shoothit : MonoBehaviour
{
    Skeleton skeleton;
    public float weaponDamage;
    [SerializeField] private LayerMask EnemyLayer;
    private float NextHitable = 0.1f;//�ߺ������� ���� �ð�
    private float HittedTime = 0f; //�����ð�
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
    }
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & EnemyLayer.value) != 0)
        {
            //�¾Ҵ� �ð� + �ߺ������� ���� �ð� <= ����ð�
            if ( (HittedTime  + NextHitable) <= Time.time)
            {

                skeleton = collision.gameObject.GetComponent<Skeleton>();
                skeleton.SetDamaged(weaponDamage);
                HittedTime = Time.time;//���� �ð� ��Ͽ�
            }
            
        }
        
    }
}
