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
            .State<State>(Idle)//전환 조건 미구현
                .Enter(state =>
                {
                    //Debug.Log($"Entering {Idle} State");
                    SetIdle();
                })
                .End()
            .State<EnemyWalkState>(Walk)//걷는 것만 구현
                .Enter(state =>
                {
                    //Debug.Log($"Entering {Walk} State");
                    SetWalk();
                })
                .Condition(() =>
                {
                    //사거리 내에 들어올 경우
                    return IsTargetReached();
                },
                state =>
                {
                    //Attack state로 전환
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
                    state.AttackCount = stat.AttackDelay;//진입시 공격
                })
                .Update((state, deltaTime) =>
                {
                    //공격 딜레이에 맞춰 공격하는 애니메이션을 실행하도록 구현
                    state.AttackCount += deltaTime;
                    if (state.AttackCount >= stat.AttackDelay)
                    {
                        state.AttackCount = 0f;
                        SetAttack();
                    }
                })
                .Condition(() =>
                {
                    //공격중이 아닌 상태 & 전환중이 아닌 상태 & 사거리를 벗어날 경우
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
                    //맞는 모션 완전히 종료 시 아래로 상태전환
                    return !IsAnimationPlaying(Damaged);
                },
                state =>
                {
                    //위의 조건 true 시, 실행될 코드 작성
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
    /// 애니메이션 중간에 가해지는 데미지, 애니메이션 클립에 해당 함수 존재
    /// </summary>
    /// <param name="physicalDamage">enemyStat의 physicalDamage를 매개변수로 추가</param>
    private void IsAttacked()
    {
        //맞는 순간 사거리 내에 있어야만 Damage
        if (IsTargetReached())
        {
            playerObject.GetComponent<Player_Health>().TakeDamage(stat.PhysicalDamage);
        }
    }
    /// <summary>
    /// 맞았을 때, 이펙트가 발생되지 않습니다.
    /// </summary>
    /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
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
    /// Raycast를 이용한 공격 시, 상호작용 가능한 메소드 입니다.
    /// 맞은 장소에 쏜 방향을 바라보는 이펙트 오브젝트가 생깁니다.
    /// </summary>
    /// <param name="hit">RaycastHit Object 넣어주세요</param>
    /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
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
            //hit.normal로 맞은 부분의 법선 벡터를 가져올 수 있습니다.
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

    //사망 시, 다른 상태전환이 되지 않도록 처리해야 합니다.
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
    /// 가라앉기 시작하고 3초 후, ObejctSpawner의 현재 소환 수 감소
    /// ObjectPool로 돌아가기
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

    //현재 미사용
    private void SetStun()
    {
        SetIdle();//Idle 재활용
        StopNavigtaion();
    }


    private void SetBoolAnimation(string state)
    {
        // 현재 애니메이터의 모든 파라미터를 가져옵니다.
        AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;

        // 각 파라미터에 대해 반복합니다.
        foreach (AnimatorControllerParameter parameter in parameters)
        {
            // 제외할 파라미터인지 확인하고, 제외되지 않은 경우 값을 false로 설정합니다.
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
    /// 공격, 데미지를 입었을 때 빠르게 기존 state로 전환하기 위함입니다.
    /// </summary>
    /// <returns>정의한 상태값의 string 값을 반환합니다.</returns>
    private string GetBoolAnimationName()
    {
        string parameterName = "";
        // 현재 애니메이터의 모든 파라미터를 가져옵니다.
        AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;
        foreach (AnimatorControllerParameter parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                //true인 파라미터면
                if (skeletonAnimator.GetBool(parameter.name))
                {
                    parameterName = parameter.name;
                }
            }

        }
        return parameterName;
    }
    /// <summary>
    /// 사정거리 내에 들어온지를 감지합니다.
    /// </summary>
    /// <returns>사정거리에 들어올 시 true</returns>
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
