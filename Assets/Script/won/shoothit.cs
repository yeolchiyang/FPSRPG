using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class shoothit : MonoBehaviour
{
    Skeleton skeleton;
    public float weaponDamage;
    [SerializeField] private LayerMask EnemyLayer;
    private float NextHitable = 0.1f;//중복데미지 방지 시간
    private float HittedTime = 0f; //맞은시간
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
            //맞았던 시간 + 중복데미지 방지 시간 <= 현재시간
            if ( (HittedTime  + NextHitable) <= Time.time)
            {

                skeleton = collision.gameObject.GetComponent<Skeleton>();
                skeleton.SetDamaged(weaponDamage);
                HittedTime = Time.time;//맞은 시간 기록용
            }
            
        }
        
    }
}
