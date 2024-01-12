using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class NormalSkeleton : Skeleton
{
    private const string Idle = "Idle";
    private const string Walk = "Walk";
    private const string Run = "Run";
    private const string Attack = "Attack";
    private const string Damaged = "Damaged";
    private const string Die = "Die";



    private void OnEnable()
    {
        Respawn();
    }
    private void Start()
    {
        rootState = new StateMachineBuilder()
            .State<State>(Idle)//��ȯ ���� �̱���
                .Enter(state =>
                {
                    //Debug.Log($"Entering {Idle} State");
                    SetIdle();
                })
                .End()
            .State<EnemyWalkState>(Walk)//�ȴ� �͸� ����
                .Enter(state =>
                {
                    //Debug.Log($"Entering {Walk} State");
                    SetWalk();
                })
                .Condition(() =>
                {
                    //��Ÿ� ���� ���� ���
                    return IsTargetReached();
                },
                state =>
                {
                    //Attack state�� ��ȯ
                    state.Parent.ChangeState(Attack);
                })
                .Event(Damaged, state =>
                {
                    state.Parent.ChangeState(Damaged);
                })
                .End()
            .State<EnemyAttackState>(Attack)
                .Enter(state =>
                {
                    //Debug.Log($"Entering {Attack} State");
                    state.AttackCount = stat.AttackDelay;//���Խ� ����
                })
                .Update((state, deltaTime) =>
                {
                    //���� �����̿� ���� �����ϴ� �ִϸ��̼��� �����ϵ��� ����
                    state.AttackCount += deltaTime;
                    if (state.AttackCount >= stat.AttackDelay)
                    {
                        state.AttackCount = 0f;
                        SetAttack();
                    }
                })
                .Condition(() =>
                {
                    //�������� �ƴ� ���� & ��ȯ���� �ƴ� ���� & ��Ÿ��� ��� ���
                    bool isInTransition = skeletonAnimator.IsInTransition(0);
                    return !IsAnimationPlaying(Attack) &&
                            !IsTargetReached() && !isInTransition;
                },
                state =>
                {
                    state.Parent.ChangeState(Walk);
                })
                .Event(Damaged, state =>
                {
                    state.Parent.ChangeState(Damaged);
                })
                .End()
            .State<State>(Damaged)
                .Enter(state =>
                {
                    //Debug.Log($"Entering {Damaged} State");
                })
                .Condition(() =>
                {
                    //�´� ��� ������ ���� �� �Ʒ��� ������ȯ
                    return !IsAnimationPlaying(Damaged);
                },
                state =>
                {
                    //���� ���� true ��, ����� �ڵ� �ۼ�
                    if (stat.CurrentHp >= 0f)
                    {
                        string stateName = GetBoolAnimationName();
                        state.Parent.ChangeState(stateName);
                    }
                })
                .End()
            .State<State>(Die)
                .Enter(state =>
                {
                    //Debug.Log($"Entering {Die} State");
                    SetDie();
                })
                .End()
            .Build();
        rootState.ChangeState(Walk);
        skeletonNav.stoppingDistance = stat.AttackRange;
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            rootState.Update(Time.fixedDeltaTime);
        }
    }
    private void SetIdle()
    {
        SetBoolAnimation(Idle);
        StopNavigtaion();
    }

    private void SetWalk()
    {
        SetBoolAnimation(Walk);
        StartNavigtaion(stat.WalkSpeed);
    }

    private void SetRun()
    {
        SetBoolAnimation(Run);
        StartNavigtaion(stat.RunSpeed);
    }

    private void SetAttack()
    {
        SetTriggerAnimation(Attack);
        StopNavigtaion();
    }
    /// <summary>
    /// �ִϸ��̼� �߰��� �������� ������, �ִϸ��̼� Ŭ���� �ش� �Լ� ����
    /// </summary>
    /// <param name="physicalDamage">enemyStat�� physicalDamage�� �Ű������� �߰�</param>
    private void IsAttacked()
    {
        //�´� ���� ��Ÿ� ���� �־�߸� Damage
        if (IsTargetReached())
        {
            playerObject.GetComponent<Player_Health>().TakeDamage(stat.PhysicalDamage);
        }
    }
    /// <summary>
    /// �¾��� ��, ����Ʈ�� �߻����� �ʽ��ϴ�.
    /// </summary>
    /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
    public override void SetDamaged(float damage)
    {
        stat.CurrentHp -= damage;
        if (stat.CurrentHp <= 0f)
        {
            rootState.ChangeState(Die);
        }
        else
        {
            rootState.TriggerEvent(Damaged);
            SetTriggerAnimation(Damaged);
            StopNavigtaion();
        }
    }
    /// <summary>
    /// Raycast�� �̿��� ���� ��, ��ȣ�ۿ� ������ �޼ҵ� �Դϴ�.
    /// ���� ��ҿ� �� ������ �ٶ󺸴� ����Ʈ ������Ʈ�� ����ϴ�.
    /// </summary>
    /// <param name="hit">RaycastHit Object �־��ּ���</param>
    /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
    public override void SetDamaged(RaycastHit hit, float damage)
    {
        stat.CurrentHp -= damage;
        if (stat.CurrentHp <= 0f)
        {
            rootState.ChangeState(Die);
        }
        else
        {
            SetTriggerAnimation(Damaged);
            StopNavigtaion();
            GameObject hitEffect = EffectPool.effectPool.GetObject(HitEffect);
            //hit.normal�� ���� �κ��� ���� ���͸� ������ �� �ֽ��ϴ�.
            hitEffect.transform.position = hit.point;
            hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
            rootState.TriggerEvent(Damaged);
        }
    }

    private bool IsAnimationPlaying(string stateName)
    {
        AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
        bool isAnimationPlaying = currentState.IsName(stateName);
        return isAnimationPlaying;
    }

    //��� ��, �ٸ� ������ȯ�� ���� �ʵ��� ó���ؾ� �մϴ�.
    public void SetDie()
    {
        this.isActive = false;
        if (!IsAnimationPlaying(Die))
        {
            SetTriggerAnimation(Die);
            StopNavigtaion();
            StartCoroutine(Sinking());
            playerObject.GetComponent<Player_Health>().ADDExp();
        }
    }
    /// <summary>
    /// ����ɱ� �����ϰ� 3�� ��, ObejctSpawner�� ���� ��ȯ �� ����
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
                GetComponent<Rigidbody>().isKinematic = false;
                ObjectSpawner.objectSpawner.CurrentSpawnedCount--;
                ObjectPool.objectPool.PoolObject(gameObject);
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void Respawn()
    {
        this.isActive = true;
        this.stat.CurrentHp = this.stat.MaxHp;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;

    }

    //���� �̻��
    private void SetStun()
    {
        SetIdle();//Idle ��Ȱ��
        StopNavigtaion();
    }


    private void SetBoolAnimation(string state)
    {
        // ���� �ִϸ������� ��� �Ķ���͸� �����ɴϴ�.
        AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;

        // �� �Ķ���Ϳ� ���� �ݺ��մϴ�.
        foreach (AnimatorControllerParameter parameter in parameters)
        {
            // ������ �Ķ�������� Ȯ���ϰ�, ���ܵ��� ���� ��� ���� false�� �����մϴ�.
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                if (parameter.name == state)
                {
                    skeletonAnimator.SetBool(parameter.name, true);
                }
                else
                {
                    skeletonAnimator.SetBool(parameter.name, false);
                }
            }
        }
    }
    /// <summary>
    /// ����, �������� �Ծ��� �� ������ ���� state�� ��ȯ�ϱ� �����Դϴ�.
    /// </summary>
    /// <returns>������ ���°��� string ���� ��ȯ�մϴ�.</returns>
    private string GetBoolAnimationName()
    {
        string parameterName = "";
        // ���� �ִϸ������� ��� �Ķ���͸� �����ɴϴ�.
        AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;
        foreach (AnimatorControllerParameter parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                //true�� �Ķ���͸�
                if (skeletonAnimator.GetBool(parameter.name))
                {
                    parameterName = parameter.name;
                }
            }

        }
        return parameterName;
    }
    /// <summary>
    /// �����Ÿ� ���� �������� �����մϴ�.
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

    private void SetTriggerAnimation(string state)
    {
        skeletonAnimator.SetTrigger(state);
    }



}
