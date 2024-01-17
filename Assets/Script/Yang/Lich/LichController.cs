using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public enum LichState
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
public class LichController : Skeleton
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
    /// <summary>
    /// Lich�� ����� �ȵǴ� ������ Trigger�� ǥ���� object �Դϴ�.
    /// </summary>
    private BoxCollider LichBoxCollider;
    public LayerMask playerMask;//�÷��̾� layer�� ���� �����Դϴ�.
    [Tooltip("Elite���� ���� �� ��ȯ�� ��ȭ�� �Դϴ�.")]
    [SerializeField] private GameObject EnhancementZone;
    [Tooltip("Lich�� �ߵ��ϴ� FireMagicObject �Դϴ�.")]
    [SerializeField] private GameObject LichFireMagic;

    ContralBossHPBar cbb;                // ������ BossHPBar ���� �߰�
    [SerializeField] GameObject BossBar; // ������ BossHPBar ���� �߰�


    private void OnEnable()
    {
        //����Ʈ�� ���� �� �Ϲ� �� ������ �Ͻ����� ��
        //�����Ǿ��ִ� �Ϲ� ���� �Ҹ��ϴ� �Լ��� �����Ͽ�, OnEnable�� �߰��ؾ� �մϴ�.
        Respawn();
    }

    private void Start()
    {
        LichBoxCollider = ObjectSpawner.objectSpawner.LichBoxCollider;
        cbb = BossBar.GetComponent<ContralBossHPBar>();  // ������ BossHPBar ���� �߰�

        rootState = new StateMachineBuilder()
            .State<State>(LichState.Idle.ToString())//�⺻ �����Դϴ�. ������ ������ �������� �ʽ��ϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {LichState.Idle.ToString()} State");
                    //1.������Ʈ Ǯ�� ���� ������ �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Idle�� �����մϴ�.
                    //3.�÷��̾� ������ �����մϴ�.(NavMesh)
                    ObjectSpawner.objectSpawner.StartSpawning();
                    SetBoolAnimation(LichState.Idle.ToString());
                    StopNavigation();
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ������ ���
                    return IsTargetDetected();
                },
                state =>
                {
                    //Walk state�� ��ȯ
                    state.Parent.ChangeState(LichState.Walk.ToString());
                })
                .End()
            .State<TreeEntWalkState>(LichState.Walk.ToString())//�����Ÿ� ���� �� Walk�� �̵��մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {LichState.Walk.ToString()} State");
                    //1.������Ʈ Ǯ�� ������� �����ϴ� �Լ��� �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Walk�� �����մϴ�.
                    //3.�÷��̾ �����մϴ�.(NavMesh)
                    //4.���ݿ� Collider�� ��Ȱ��ȭ �մϴ�.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(LichState.Walk.ToString());

                    BossBar.SetActive(true);  // ������ BossHPBar ���� �߰�
                    cbb.setBossInfo(stat.CurrentHp, stat.MaxHp, stat.Name);  // ������ BossHPBar ���� �߰�

                    StartNavigation(stat.WalkSpeed);
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
                        StartNavigation(stat.WalkSpeed);
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
                        StartNavigation(stat.WalkSpeed, LichBoxCollider.transform.position);
                    }
                    else
                    {
                        StopNavigation();
                    }
                })
                .Condition(() =>
                {
                    //navmesh �������� �߾��̸�, �߾ӿ� ������ ��� true
                    return IsEntInCenter();
                },
                state =>
                {
                    state.Parent.ChangeState(LichState.Idle.ToString());
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
                        state.Parent.ChangeState(LichState.Attack.ToString());
                        state.AttackedTime = Time.time;
                    }
                })
                .End()
            .State<State>(LichState.Attack.ToString())//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {LichState.Attack.ToString()} State");
                    //1.�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                    //2.NavMesh�� �����մϴ�.
                    //3.���ݿ� Collider�� Ȱ��ȭ �մϴ�.
                    SetTriggerAnimation(LichState.Attack.ToString());
                    StopNavigation();
                    ToggleAttackCollider(true);
                })
                .End()
            .State<State>(LichState.Enranged.ToString())//���� �� ���Ϸ� �������� �ߵ��ϴ� ����ȭ �Դϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {LichState.Enranged.ToString()} State");
                    //����ȭ ��� ��, Idle�� ���ư��ϴ�.
                    SetTriggerAnimation(LichState.Enranged.ToString());
                    StopNavigation();
                })
                .End()
            .State<State>(LichState.EnrangedIdle.ToString())//����ȭ ������ Idle�Դϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {LichState.EnrangedIdle.ToString()} State");
                    SetBoolAnimation(LichState.EnrangedIdle.ToString());
                    StopNavigation();
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ������ ���
                    return IsTargetDetected();
                },
                state =>
                {
                    //Run state�� ��ȯ
                    state.Parent.ChangeState(LichState.Run.ToString());
                })
                .End()
            .State<TreeEntWalkState>(LichState.Run.ToString())//����ȭ ���¿��� �����Ÿ� ���� �� Run���� �̵��մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {LichState.Run.ToString()} State");
                    //1.������Ʈ Ǯ�� ������� �����ϴ� �Լ��� �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Walk�� �����մϴ�.
                    //3.�÷��̾ �����մϴ�.(NavMesh)
                    //4.���ݿ� Collider�� ��Ȱ��ȭ �մϴ�.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(LichState.Run.ToString());
                    StartNavigation(stat.RunSpeed);
                    ToggleAttackCollider(false);
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���̸�, Trigger�� �������� �ʾ��� �� true
                    return IsTargetDetected();
                },
                state =>
                {
                    //���� �ֱ⸶��, �������� �ʱ�ȭ �˴ϴ�.
                    if ((state.LastResetDestinationTime + stat.ResetDestinationDelay) <= Time.time)
                    {
                        StartNavigation(stat.RunSpeed);
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
                    state.Parent.ChangeState(LichState.EnrangedIdle.ToString());
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
                        state.Parent.ChangeState(LichState.EnrangedAttack.ToString());
                        state.AttackedTime = Time.time;
                    }
                })
                .End()
            .State<State>(LichState.EnrangedAttack.ToString())//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {LichState.EnrangedAttack.ToString()} State");
                    //�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                    //���ݿ� Collider�� Ȱ��ȭ �մϴ�.
                    SetTriggerAnimation(LichState.EnrangedAttack.ToString());
                    ToggleAttackCollider(true);
                })
                .End()
            .State<State>(LichState.Die.ToString())//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {LichState.Die.ToString()} State");
                    //�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                    //���ݿ� Collider�� Ȱ��ȭ �մϴ�.
                    SetDie();
                })
                .End()
            .Build();

        rootState.ChangeState(LichState.Idle.ToString());//�ʱ� ���� Idle
    }

    /// <summary>
    /// �¾��� ��, ����Ʈ�� �߻����� �ʽ��ϴ�.
    /// </summary>
    /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
    public override void SetDamaged(float damage)
    {
        if (IsInvulnerable)//��������
        {
            return;
        }
        cbb.TakeDamage(damage);
        stat.CurrentHp -= damage;

        if (stat.CurrentHp / stat.MaxHp <= 0.3f)
        {
            //����ȭ�� �� �ִ� ���°����� �����մϴ�.
            if (!isEnranged)
            {
                isEnranged = true;
                rootState.ChangeState(LichState.Enranged.ToString());
            }
        }

        if (stat.CurrentHp <= 0f)
        {
            if (isActive)//�ѹ� ������ �ٲ�� ���°��Դϴ�.
            {
                //��� �� ó���� ������ ���� �޼ҵ带 ����ֽ��ϴ�.
                this.isActive = false;
                rootState.ChangeState(LichState.Die.ToString());
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
            return;
        }
        stat.CurrentHp -= damage;
        if (stat.CurrentHp / stat.MaxHp <= 0.3f)
        {
            //����ȭ�� �� �ִ� ���°����� �����մϴ�.
            if (!isEnranged)
            {
                isEnranged = true;
                rootState.ChangeState(LichState.Enranged.ToString());
            }
        }

        if (stat.CurrentHp <= 0f)
        {
            if (isActive)//�ѹ� ������ �ٲ�� ���°��Դϴ�.
            {
                //��� �� ó���� ������ ���� �޼ҵ带 ����ֽ��ϴ�.
                this.isActive = false;
                rootState.ChangeState(LichState.Die.ToString());
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
    /// ��ġ�� �ߵ��ϴ� �Ҹ��� �Դϴ�.
    /// </summary>
    public void ExecuteMagicAttack()
    {
        Debug.Log("��ȯ��");
        GameObject LichFireObject = EffectPool.effectPool.GetObject(LichFireMagic);
        LichFireMagic.transform.rotation = transform.rotation;
        LichFireMagic.GetComponent<LichFireMagic>().SetFireDamage(stat.PhysicalDamage * 2);
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
        Vector3 navEndPosition = new Vector3(skeletonNav.destination.x,
                                            0, skeletonNav.destination.z);
        Vector3 TreeEntSpawnPosition = new Vector3(LichBoxCollider.transform.position.x,
                                    0, LichBoxCollider.transform.position.z);
        if (Vector3.Distance(navEndPosition, TreeEntSpawnPosition) < 2f)
        {
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
        Vector3 minPosition = LichBoxCollider.bounds.min;//�ּ� ��ǥ
        Vector3 maxPosition = LichBoxCollider.bounds.max;//�ִ� ��ǥ
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
    /// ��ȭ ��Ż�� �����ϴ�.
    /// </summary>
    public void SetDie()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        if (!IsAnimationPlaying(LichState.Die.ToString()))
        {
            SetTriggerAnimation(LichState.Die.ToString());
            StopNavigation();
            EnhancementZone = ObjectPool.objectPool.GetObject(EnhancementZone);//��ȭ ��Ż ��ȯ
            EnhancementZone.transform.position = new Vector3(transform.position.x, playerObject.transform.position.y, transform.position.z);
            StartCoroutine(Sinking());
            ObjectPool.objectPool.IncrementBossRoomCount(this);//��ȭ�� ������ ���ÿ�, ������ ���� ���� 1�� ����
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
