using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yang;

public enum TreeState
{
    Idle,//bool
    Walk,//bool
    Attack,//Trigger
    Enranged,//Trigger
    EnrangedIdle,//bool
    Run,//bool
    Damaged,
    Die,//Trigger
    EnrangedAttack,//Trigger
    StompAttack,//Trigger
    JumpSmashAttack//Trigger
}

public class TreeEntController : Skeleton
{

    ///�������� ���θ� üũ�մϴ�.
    public bool IsInvulnerable { get; set; } = false;
    /// <summary>
    /// �г���� ���θ� �����մϴ�.
    /// </summary>
    private bool isEnranged = false;
    [Tooltip("���� ������ ���� Capsule Collider�� ����ֽ��ϴ�. " +
        "������� Collider�� ���̴� �⺻���� ��Ÿ��� 2�踸ŭ�� ũ�Ⱑ �˴ϴ�.")]
    [SerializeField] private CapsuleCollider[] AttackColliders;
    [Tooltip("Player���� �������� ���� �� �浹�� �Ͼ ��ǥ�� �����Ǵ� ����Ʈ�Դϴ�")]
    [SerializeField] public GameObject NormalHitEffect;
    [Tooltip("TreeEnt�� ����� �ȵǴ� ������ Trigger�� ǥ���� object �Դϴ�.")]
    [SerializeField] private BoxCollider TreeEntBoxCollider;

    public LayerMask playerMask;//�÷��̾� layer�� ���� �����Դϴ�.
    


    private void OnEnable()
    {
        //����Ʈ�� ���� �� �Ϲ� �� ������ �Ͻ����� ��
        //�����Ǿ��ִ� �Ϲ� ���� �Ҹ��ϴ� �Լ��� �����Ͽ�, OnEnable�� �߰��ؾ� �մϴ�.
        Respawn();
        //transform.parent.SendMessage()
    }

    private void Start()
    {
        rootState = new StateMachineBuilder()
            .State<State>(TreeState.Idle.ToString())//�⺻ �����Դϴ�. ������ ������ �������� �ʽ��ϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Idle.ToString()} State");
                    //1.������Ʈ Ǯ�� ���� ������ �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Idle�� �����մϴ�.
                    //3.�÷��̾� ������ �����մϴ�.(NavMesh)
                    ObjectSpawner.objectSpawner.StartSpawning();
                    SetBoolAnimation(TreeState.Idle.ToString());
                    StopNavigtaion();
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ������ ���
                    return IsTargetDetected();
                },
                state =>
                {
                    //Walk state�� ��ȯ
                    state.Parent.ChangeState(TreeState.Walk.ToString());
                })
                .End()
            .State<TreeEntWalkState>(TreeState.Walk.ToString())//�����Ÿ� ���� �� Walk�� �̵��մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Walk.ToString()} State");
                    //1.������Ʈ Ǯ�� ������� �����ϴ� �Լ��� �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Walk�� �����մϴ�.
                    //3.�÷��̾ �����մϴ�.(NavMesh)
                    //4.���ݿ� Collider�� ��Ȱ��ȭ �մϴ�.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(TreeState.Walk.ToString());
                    StartNavigtaion(stat.WalkSpeed);
                    ToggleAttackCollider(false);
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���̸�, Trigger�� �������� �ʾ��� �� true
                    return IsTargetDetected() && !IsOutsideBounds();
                },
                state =>
                {
                    //���� �ֱ⸶��, �������� �ʱ�ȭ �˴ϴ�.
                    if ((state.LastResetDestinationTime + stat.ResetDestinationDelay) <= Time.time)
                    {
                        StartNavigtaion(stat.WalkSpeed);
                        state.LastResetDestinationTime = Time.time;
                    }
                })
                .Condition(() =>
                {
                    //Trigger�� �����Ǿ����� �ϴ� true�� ��ȯ 
                    return IsOutsideBounds();
                },
                state =>
                {
                    /* must======
                     * 1. Trigger�� �����ǰ�, �÷��̾ �����Ÿ� ���� ��� ->
                     * ���ư���(navMesh�� destination ����)
                     * 2. Trigger�� �����ǰ�, �÷��̾ �����Ÿ� ���� ��� -> 
                     * ��� Walk, StopNavigation
                     */
                    if (!IsTargetDetected()) //�÷��̾ �����Ÿ� ���� ���
                    {
                        StartNavigation(stat.WalkSpeed, TreeEntBoxCollider.transform.position);
                    }
                    else
                    {
                        StopNavigtaion();
                    }
                })
                .Condition(() =>
                {
                    //navmesh �������� �߾��̸�, �߾ӿ� ������ ��� true
                    return IsEntInCenter();
                },
                state =>
                {
                    state.Parent.ChangeState(TreeState.Idle.ToString());
                })
                .Condition(() =>
                {
                    //���� ��Ÿ� ���� ������ ���
                    return IsTargetReached();
                },
                state =>
                {
                    //���� ��Ÿ���� ������ ��쿡�� Attack state�� ��ȯ�մϴ�.
                    if ((state.AttackedTime + stat.AttackDelay) <= Time.time)
                    {
                        state.Parent.ChangeState(TreeState.Attack.ToString());
                        state.AttackedTime = Time.time;
                    }
                })
                .End()
            .State<State>(TreeState.Attack.ToString())//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Attack.ToString()} State");
                    //1.�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                    //2.NavMesh�� �����մϴ�.
                    //3.���ݿ� Collider�� Ȱ��ȭ �մϴ�.
                    SetTriggerAnimation(TreeState.Attack.ToString());
                    StopNavigtaion();
                    ToggleAttackCollider(true);
                })
                .End()
            .State<State>(TreeState.Enranged.ToString())//���� �� ���Ϸ� �������� �ߵ��ϴ� ����ȭ �Դϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Enranged.ToString()} State");
                    //����ȭ ��� ��, Idle�� ���ư��ϴ�.
                    SetTriggerAnimation(TreeState.Enranged.ToString());
                    StopNavigtaion();
                })
                .End()
            .State<State>(TreeState.EnrangedIdle.ToString())//����ȭ ������ Idle�Դϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.EnrangedIdle.ToString()} State");
                    SetBoolAnimation(TreeState.EnrangedIdle.ToString());
                    StopNavigtaion();
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ������ ���
                    return IsTargetDetected();
                },
                state =>
                {
                    //Run state�� ��ȯ
                    state.Parent.ChangeState(TreeState.Run.ToString());
                })
                .End()
            .State<TreeEntWalkState>(TreeState.Run.ToString())//����ȭ ���¿��� �����Ÿ� ���� �� Run���� �̵��մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Run.ToString()} State");
                    //1.������Ʈ Ǯ�� ������� �����ϴ� �Լ��� �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Walk�� �����մϴ�.
                    //3.�÷��̾ �����մϴ�.(NavMesh)
                    //4.���ݿ� Collider�� ��Ȱ��ȭ �մϴ�.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(TreeState.Run.ToString());
                    StartNavigtaion(stat.RunSpeed);
                    ToggleAttackCollider(false);
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���̸�, Trigger�� �������� �ʾ��� �� true
                    Debug.Log("������ �Դϴ�.");
                    return IsTargetDetected();
                },
                state =>
                {
                    //���� �ֱ⸶��, �������� �ʱ�ȭ �˴ϴ�.
                    Debug.Log("�����Ÿ� ���Դϴ�.");
                    if ((state.LastResetDestinationTime + stat.ResetDestinationDelay) <= Time.time)
                    {
                        StartNavigtaion(stat.RunSpeed);
                        state.LastResetDestinationTime = Time.time;
                    }
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ����� ���
                    return !IsTargetDetected();
                },
                state =>
                {
                    //Idle state�� ��ȯ
                    state.Parent.ChangeState(TreeState.EnrangedIdle.ToString());
                })
                .Condition(() =>
                {
                    //���� ��Ÿ� ���� ������ ���
                    return IsTargetReached();
                },
                state =>
                {
                    //���� ��Ÿ���� ������ ��쿡�� Attack state�� ��ȯ�մϴ�.
                    if ((state.AttackedTime + stat.AttackDelay) <= Time.time)
                    {
                        state.Parent.ChangeState(TreeState.EnrangedAttack.ToString());
                        state.AttackedTime = Time.time;
                    }
                })
                .End()
            .State<State>(TreeState.EnrangedAttack.ToString())//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.EnrangedAttack.ToString()} State");
                    //�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                    //���ݿ� Collider�� Ȱ��ȭ �մϴ�.
                    SetTriggerAnimation(TreeState.EnrangedAttack.ToString());
                    ToggleAttackCollider(true);
                })
                .End()
            .State<State>(TreeState.Die.ToString())//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Die.ToString()} State");
                    //�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                    //���ݿ� Collider�� Ȱ��ȭ �մϴ�.
                    Debug.Log("���׾�");
                    SetDie();
                })
                .End()
            .Build();

        rootState.ChangeState(TreeState.Idle.ToString());//�ʱ� ���� Idle
    }

    /// <summary>
    /// �¾��� ��, ����Ʈ�� �߻����� �ʽ��ϴ�.
    /// </summary>
    /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
    public override void SetDamaged(float damage)
    {
        if(IsInvulnerable)//��������
        {
            Debug.Log("�����Դϴ�.");
            return;
        }
        Debug.Log("damage����");
        stat.CurrentHp -= damage;

        if( stat.CurrentHp / stat.MaxHp <= 0.3f )
        {
            //����ȭ�� �� �ִ� ���°����� �����մϴ�.
            if(!isEnranged)
            {
                rootState.ChangeState(TreeState.Enranged.ToString());
            }
        }

        if (stat.CurrentHp <= 0f)
        {
            if (isActive)//�ѹ� ������ �ٲ�� ���°��Դϴ�.
            {
                //��� �� ó���� ������ ���� �޼ҵ带 ����ֽ��ϴ�.
                rootState.ChangeState(TreeState.Die.ToString());
            }
        }

    }
    /// <summary>
    /// Raycast�� �̿��� ���� ��, ��ȣ�ۿ� ������ �޼ҵ� �Դϴ�.
    /// ���� ��ҿ� ����Ʈ�� ����ϴ�.
    /// </summary>
    /// <param name="hit">RaycastHit Object �־��ּ���</param>
    /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
    public override void SetDamaged(RaycastHit hit, float damage)
    {
        if (IsInvulnerable)//��������
        {
            Debug.Log("�����Դϴ�.");
            return;
        }
        Debug.Log("damage����");
        stat.CurrentHp -= damage;
        if (stat.CurrentHp / stat.MaxHp <= 0.3f)
        {
            //����ȭ�� �� �ִ� ���°����� �����մϴ�.
            if (!isEnranged)
            {
                rootState.ChangeState(TreeState.Enranged.ToString());
            }
        }

        if (stat.CurrentHp <= 0f)
        {
            if(isActive)//�ѹ� ������ �ٲ�� ���°��Դϴ�.
            {
                //��� �� ó���� ������ ���� �޼ҵ带 ����ֽ��ϴ�.
                rootState.ChangeState(TreeState.Die.ToString());
            }
        }
    }


    /// <summary>
    /// �Ϲݰ��� 3���� �����ϴ� ������ ������ �޼ҵ� �Դϴ�.
    /// </summary>
    public void IsAttacked()
    {
        playerObject.GetComponent<Player_Health>().TakeDamage(stat.PhysicalDamage);
    }

    /// <summary>
    /// �ȿ� �޸� �� Collider�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �޼ҵ� �Դϴ�.
    /// Idle State ��, Collider�� ��Ȱ��ȭ �մϴ�. 
    /// Attack State ��, Collider�� Ȱ��ȭ �մϴ�.
    /// </summary>
    private void ToggleAttackCollider(bool changeToggle)
    {
        foreach (Collider attackCollider in AttackColliders)
        {
            attackCollider.enabled = changeToggle;
        }
    }
    /// <summary>
    /// Ent�� �߾ӿ� �ִ����� �����մϴ�.
    /// </summary>
    /// <returns>�߾ӿ� ������ ��, true�� ��ȯ�մϴ�.</returns>
    private bool IsEntInCenter()
    {
        bool isEntInCenter = false;
        Vector3 navEndPosition = new Vector3(skeletonNav.pathEndPosition.x, 
                                            0, skeletonNav.pathEndPosition.z);
        Vector3 TreeEntSpawnPosition = new Vector3(TreeEntBoxCollider.transform.position.x,
                                    0, TreeEntBoxCollider.transform.position.z);
        Debug.Log(Vector3.Distance(navEndPosition, TreeEntSpawnPosition));
        if(Vector3.Distance(navEndPosition, TreeEntSpawnPosition) < 2f)
        {
            Debug.Log(skeletonNav.remainingDistance);
            if (skeletonNav.remainingDistance <= 1f)
            {
                isEntInCenter = true;
            }
        }
        return isEntInCenter;
    }

    /// <summary>
    /// �����Ÿ� ���� �������� �Ǵ��մϴ�.
    /// </summary>
    /// <returns>�����Ÿ� ���� ���� �� true</returns>
    private bool IsTargetDetected()
    {
        bool isTargetDetected = false;
        Vector3 enemyXZ = new Vector3(transform.position.x,
                                        0, transform.position.z);
        Vector3 playerXZ = new Vector3(playerObject.transform.position.x,
                                        0, playerObject.transform.position.z);
        float distanceToPlayer = Vector3.Distance(
                        playerXZ, enemyXZ);
        if (distanceToPlayer <= stat.DetectionRange)
        {
            isTargetDetected = true;
        }
        return isTargetDetected;
    }


    /// <summary>
    /// �����Ÿ� ���� �������� �����մϴ�.
    /// ������ ���� ���� �ʿ��� �� �����մϴ�
    /// </summary>
    /// <returns>�����Ÿ��� ���� �� true</returns>
    private bool IsTargetReached()
    {
        bool isTargetReached = false;
        Vector3 enemyXZ = new Vector3(transform.position.x,
                                0, transform.position.z);
        Vector3 playerXZ = new Vector3(playerObject.transform.position.x,
                                        0, playerObject.transform.position.z);
        float distanceToPlayer = Vector3.Distance(
                        playerXZ, enemyXZ);
        if (distanceToPlayer <= stat.AttackRange)
        {
            isTargetReached = true;
        }
        return isTargetReached;
    }

    /// <summary>
    /// SpawnPoint�� Collider ������ ���������� �Ǵ��մϴ�.
    /// </summary>
    /// <returns>�������� true�� ��ȯ</returns>
    private bool IsOutsideBounds()
    {
        bool isOutsideBounds = false;
        Vector3 minPosition = TreeEntBoxCollider.bounds.min;//�ּ� ��ǥ
        Vector3 maxPosition = TreeEntBoxCollider.bounds.max;//�ִ� ��ǥ
        if (transform.position.x <= minPosition.x || transform.position.x >= maxPosition.x
        || transform.position.z <= minPosition.z || transform.position.z >= maxPosition.z)
        {
            isOutsideBounds = true;
        }
        return isOutsideBounds;
    }

    /// <summary>
    /// ���� ��ȯ�� �� ����˴ϴ�.
    /// ���� ��Ÿ� * 2 ��ŭ capsule collider ũ�⸦ �����մϴ�.
    /// Active �Ӽ� : ����ִ����� �Ǻ��մϴ�.
    /// </summary>
    private void Respawn()
    {
        this.isActive = true;
        this.stat.CurrentHp = this.stat.MaxHp;
        GetComponent<CapsuleCollider>().enabled = true;
        foreach (CapsuleCollider capsule in AttackColliders)
        {
            capsule.height = stat.AttackRange * 2;
        }
        playerMask = playerObject.layer;
    }
    /// <summary>
    /// ��� ��, ����ɴ� �ڷ�ƾ�� ����˴ϴ�.
    /// ��ȭ ����Ʈ�� ����մϴ�.
    /// </summary>
    public void SetDie()
    {
        this.isActive = false;
        GetComponent<CapsuleCollider>().enabled = false;
        if (!IsAnimationPlaying(TreeState.Die.ToString()))
        {
            SetTriggerAnimation(TreeState.Die.ToString());
            StopNavigtaion();
            StartCoroutine(Sinking());
            //playerObject.GetComponent<Player_Health>().ADDExp();
        }
    }

    /// <summary>
    /// ����ɱ� �����ϰ� 3�� ��
    /// ObjectPool�� ���ư���
    /// </summary>
    /// <returns></returns>
    private IEnumerator Sinking()
    {
        yield return new WaitForSeconds(1f);
        float timer = 0f;
        float sinkingStartTime = 3.4f;
        float destroyTime = 5f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > sinkingStartTime)
            {
                transform.position += Vector3.down * Time.fixedDeltaTime * 0.5f;
            }
            if (timer > destroyTime)
            {
                GetComponent<CapsuleCollider>().enabled = false;
                ObjectPool.objectPool.PoolObject(gameObject);
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }



}
