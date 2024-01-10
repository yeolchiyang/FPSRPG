using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYDRABoss : MonoBehaviour
{
    HYDRA hydra;
    HYDRAStat stat;
    [SerializeField] Transform player;
    float rotationSpeed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        stat = GetComponent<HYDRAStat>();

    }

    float normalTimer = 3f;
    float skillTimer = 5f;
    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if(distanceToPlayer <= stat.SkillattackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveToPoint();
            }
        }

    }

    void MoveTowardsPlayer()
    {
        Vector3 targetPosition = player.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, stat.WalkSpeed * Time.deltaTime);

        Vector3 direction = (targetPosition-transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    [SerializeField] Transform[] points;
    int currentpointIndex = 0;

    void MoveToPoint()
    {
        if(currentpointIndex < points.Length)
        {
            Vector3 targetPosition = points[currentpointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, stat.WalkSpeed * Time.deltaTime);
            if(transform.position == targetPosition)
            {
                currentpointIndex++;
                if(currentpointIndex >= points.Length)
                {
                    currentpointIndex = 0;
                }
            }

            Vector3 direction = (targetPosition-transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }


}


// 플레이어가 없을 떄 기본적인 이동
// + 플레이어를 바라보며 or 가는 방향을 바라보며 이동



// 정해진 간격으로 일반공격 ..?   // HeavyHit, Grab, Fivehit, Threehit
// 정해진 간격으로 스킬공격 ..?   // Roar
// 스킬 공격시 바닥에 공격하는 구간 표시 ( 공격 전 정해진 시간동안 표시 후 공격시작과 동시에 없애야 함 )
// 플레이어의 공격과 보스체력의 상호작용
// 보스의 공격과 플레이어체력의 상호작용
// 보스 공격  중 Roar 활성화 시 파티클 추가
// 보스의 공격이 오브젝트와 충돌시 파티클 제거
