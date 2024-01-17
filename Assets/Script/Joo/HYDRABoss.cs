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
    [SerializeField] GameObject BossBar; // ������ BossHPBar ���� �߰�
    ContralBossHPBar cbb;                // ������ BossHPBar ���� �߰�
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
        cbb = BossBar.GetComponent<ContralBossHPBar>();  // ������ BossHPBar ���� �߰�
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
            BossBar.SetActive(true);  // ������ BossHPBar ���� �߰�
            cbb.setBossInfo(stat.CurrentHp, stat.MaxHp, stat.Name);  // ������ BossHPBar ���� �߰�
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


// �÷��̾ ���� �� �⺻���� �̵�
// + �÷��̾ �ٶ󺸸� or ���� ������ �ٶ󺸸� �̵�



// ������ �������� �Ϲݰ��� ..?   // HeavyHit, Fivehit, Threehit
// ������ �������� ��ų���� ..?   // Roar
/*
     normalattackDelay
     skillattackDelay

     �׻� �� �ð��� �������� üũ
     �Ѵ� ���Ҵ� -> �켱���� ���� -> ���� �ϳ� �������̴� (�ٸ� �ϳ� �𵹾Ƶ� ���� ���ϰ� ����, bool ������ ���� ����) -> ������ ������(��Ÿ�� �� �� ����)
 */


// ���� ����  �� Roar Ȱ��ȭ �� ��ƼŬ �߰�
// ��ƼŬ�� 


// ��ų ���ݽ� �ٴڿ� �����ϴ� ���� ǥ�� ( ���� �� ������ �ð����� ǥ�� �� ���ݽ��۰� ���ÿ� ���־� �� )



// �÷��̾��� ���ݰ� ����ü���� ��ȣ�ۿ�
// ������ ���ݰ� �÷��̾�ü���� ��ȣ�ۿ�
// ������ ������ ������Ʈ�� �浹�� ��ƼŬ ����
