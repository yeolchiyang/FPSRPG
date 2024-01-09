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
        //엘리트몹 등장 시 일반 몹 리스폰 일시정지 및 생성되어있는 일반 몹 소멸
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
