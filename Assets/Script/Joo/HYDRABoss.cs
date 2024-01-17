using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SearchService;

public class HYDRABoss : MonoBehaviour
{
    HYDRA hydra;
    HYDRAStat stat;
    [SerializeField] Transform player;
    [SerializeField] GameObject BossBar; // 진선윤 BossHPBar 조인 추가
    ContralBossHPBar cbb;                // 진선윤 BossHPBar 조인 추가
    float rotationSpeed = 5f;
    protected UnityEngine.AI.NavMeshAgent Nav;
    Player_Health Playercc;

    ParticleSystem bress;

    void Start()
    {
        Nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        stat = GetComponent<HYDRAStat>();
        hydra = GetComponent<HYDRA>();
        StartCoroutine("NextMove");
        stat.CurrentHp = stat.MaxHp;
        cbb = BossBar.GetComponent<ContralBossHPBar>();  // 진선윤 BossHPBar 조인 추가
        Playercc = player.GetComponent<Player_Health>();
    }

    bool NextMove_is_running = false;

    //stat.SkillattackDelay = 8f
    float skillTimer = 0f;

    void Update()
    {
        if (!hydra.IsActivecheck())
            return;

        skillTimer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(distanceToPlayer <= stat.SkillattackRange)
        {
            StopCoroutine("NextMove");
            NextMove_is_running = false;
            BossBar.SetActive(true);  // 진선윤 BossHPBar 조인 추가
            cbb.setBossInfo(stat.CurrentHp, stat.MaxHp, stat.Name);  // 진선윤 BossHPBar 조인 추가
            MoveTowardsPlayer();
        }
        else
        {
            if(NextMove_is_running == false)
            {
                StartCoroutine("NextMove");
            }
        }

    }

    void MoveTowardsPlayer()
    {
        if (Nav.enabled == false)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Vector3 targetPosition = player.position;
        Nav.SetDestination(targetPosition);

        if (skillTimer > stat.AttackDelay)
        {
            skillTimer = 0f;

            if (Random.Range(0, 100) <= 25)
            {
                if(distanceToPlayer <= stat.SkillattackRange)
                {
                    hydra.SetRoar();
                    StartCoroutine("Playerburn");
                    //StartCoroutine("Attackanim");
                }
            }
            else if (Random.Range(0, 100) > 25 && Random.Range(0, 100) <= 50)
            {
                if (distanceToPlayer <= stat.NormalAttackRange)
                {
                    hydra.SetHeavyHit();
                    StartCoroutine("Playerbosscc");
                    //StartCoroutine("Attackanim");
                }
            }
            else if (Random.Range(0, 100) > 50 && Random.Range(0, 100) <= 75)
            {
                if (distanceToPlayer <= stat.NormalAttackRange)
                {
                    hydra.SetThreehit();
                    StartCoroutine("Attackanim");
                }
            }
            else
            {
                if (distanceToPlayer <= stat.NormalAttackRange)
                {
                    hydra.SetFivehit();
                    StartCoroutine("Attackanim");
                }
            }
        }

        if (distanceToPlayer > stat.SkillattackRange)
        {
            StartCoroutine("NextMove");
        }
    }

    IEnumerator Attackanim()
    {
        Nav.enabled = false;
        yield return new WaitForSeconds(5f);
        Nav.enabled = true;
    }

    IEnumerator Playerburn()
    {
        Nav.enabled = false;
        Playercc.PlayerCcon(0);
        yield return new WaitForSeconds(3f);
        Nav.enabled = true;
    }

    IEnumerator Playerbosscc()
    {
        Nav.enabled = false;
        yield return new WaitForSeconds(2f);
        Playercc.PlayerCcon(3);
        yield return new WaitForSeconds(3f);
        Nav.enabled = true;
    }

    [SerializeField] Transform[] points;
    int currentpointIndex = 0;

    IEnumerator NextMove()
    {
        if (Nav.enabled == false)
            yield return null;

        NextMove_is_running = true;
        Nav.SetDestination(points[currentpointIndex].position);
        hydra.SetWalk(true);
        while (true)
        {
            if(Vector3.Distance(transform.position, points[currentpointIndex].position) < 10)
            {
                currentpointIndex++;
                if (currentpointIndex >= points.Length)
                {
                    currentpointIndex = 0;
                }

                Nav.SetDestination(transform.position);
                hydra.SetWalk(false);
                yield return new WaitForSeconds(1f);

                Vector3 targetPosition = points[currentpointIndex].position;
                Nav.SetDestination(targetPosition);
                hydra.SetWalk(true);
            }
            yield return new WaitForEndOfFrame();
        }
    
    }



}


// 플레이어가 없을 떄 기본적인 이동
// + 플레이어를 바라보며 or 가는 방향을 바라보며 이동



// 정해진 간격으로 일반공격 ..?   // HeavyHit, Fivehit, Threehit
// 정해진 간격으로 스킬공격 ..?   // Roar
/*
     normalattackDelay
     skillattackDelay

     항상 저 시간이 지났는지 체크
     둘다 돌았다 -> 우선순위 결정 -> 무언가 하나 실행중이다 (다른 하나 쿨돌아도 실행 못하게 막기, bool 변수로 제어 가능) -> 실행이 끝났다(쿨타임 돈 것 실행)
 */


// 보스 공격  중 Roar 활성화 시 파티클 추가
// 파티클과 


// 스킬 공격시 바닥에 공격하는 구간 표시 ( 공격 전 정해진 시간동안 표시 후 공격시작과 동시에 없애야 함 )



// 플레이어의 공격과 보스체력의 상호작용
// 보스의 공격과 플레이어체력의 상호작용
// 보스의 공격이 오브젝트와 충돌시 파티클 제거
