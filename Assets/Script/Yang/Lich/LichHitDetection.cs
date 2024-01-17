using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichHitDetection : MonoBehaviour
{
    public float NextHittedTime { get; set; } = 0.1f;//�ѹ� ���� �� �ٽ� ���� �� �ִ� �ð�
    public float CurrentHittedTime { get; set; } = 0f;
    private LayerMask playerMask;//�θ� Object�κ��� ������ playerMask
    private LichController parentController;

    private void OnEnable()
    {
        parentController = GetComponentInParent<LichController>();
        playerMask = GetComponentInParent<LichController>().playerMask;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CurrentHittedTime + NextHittedTime <= Time.time)//���� Ÿ�� ����
        {
            int collidedLayer = collision.gameObject.layer;
            int playerMaskValue = playerMask.value;
            if (((1 << collidedLayer) & playerMaskValue) != 0)
            {
                CurrentHittedTime = Time.time;
                //player ������ ������ ������ų �� �ִ� �޼ҵ带 ���� ��!!!!
                parentController.IsAttacked();
                GameObject spawnEffect = EffectPool.effectPool.GetObject(parentController.NormalHitEffect);
                spawnEffect.transform.position = collision.contacts[0].point;//�� ó�� �浹 ��ǥ
                spawnEffect.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
            }
        }
    }
}
