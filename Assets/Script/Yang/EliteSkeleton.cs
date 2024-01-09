using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class EliteSkeleton : Skeleton
{
    

    private void OnEnable()
    {
        //Respawn();
        //����Ʈ�� ���� �� �Ϲ� �� ������ �Ͻ����� �� �����Ǿ��ִ� �Ϲ� �� �Ҹ�
    }
    private void Start()
    {
        
        skeletonNav.stoppingDistance = stat.AttackRange;
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            rootState.Update(Time.fixedDeltaTime);
        }
    }

}
