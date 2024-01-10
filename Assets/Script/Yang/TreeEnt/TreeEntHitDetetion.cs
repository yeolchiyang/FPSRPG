using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntHitDetetion : MonoBehaviour
{
    public float NextHittedTime { get; set; } = 0.3f;//�ѹ� ���� �� �ٽ� ���� �� �ִ� �ð�
    public float CurrentHittedTime { get; set; } = 0f;
    private LayerMask playerMask;

    private void OnEnable()
    {
        playerMask = GetComponentInParent<TreeEntController>().playerMask;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CurrentHittedTime + NextHittedTime <= Time.time)
        {
            int collidedLayer = other.gameObject.layer;
            int playerMaskValue = 1 << playerMask.value;
            if (((1 << collidedLayer) & playerMaskValue) != 0)
            {
                CurrentHittedTime = Time.time;
                //player ������ ������ ������ų �� �ִ� �޼ҵ带 ���� ��!!!!
            }
        }
    }
}
