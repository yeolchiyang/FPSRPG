using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichHitDetection : MonoBehaviour
{
    public float NextHittedTime { get; set; } = 0.1f;//한번 맞은 뒤 다시 때릴 수 있는 시간
    public float CurrentHittedTime { get; set; } = 0f;
    private LayerMask playerMask;//부모 Object로부터 가져올 playerMask
    private LichController parentController;

    private void OnEnable()
    {
        parentController = GetComponentInParent<LichController>();
        playerMask = GetComponentInParent<LichController>().playerMask;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CurrentHittedTime + NextHittedTime <= Time.time)//연속 타격 방지
        {
            int collidedLayer = collision.gameObject.layer;
            int playerMaskValue = playerMask.value;
            if (((1 << collidedLayer) & playerMaskValue) != 0)
            {
                CurrentHittedTime = Time.time;
                //player 데미지 입음을 실현시킬 수 있는 메소드를 넣을 것!!!!
                parentController.IsAttacked();
                GameObject spawnEffect = EffectPool.effectPool.GetObject(parentController.NormalHitEffect);
                spawnEffect.transform.position = collision.contacts[0].point;//맨 처음 충돌 좌표
                spawnEffect.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
            }
        }
    }
}
