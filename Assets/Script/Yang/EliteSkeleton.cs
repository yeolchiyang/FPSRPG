using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class EliteSkeleton : Skeleton
{
    [SerializeField]

    private void OnEnable()
    {
        //Respawn();
        //엘리트몹 등장 시 일반 몹 리스폰 일시정지 및 생성되어있는 일반 몹 소멸
    }
    private void Start()
    {
        rootState = new StateMachineBuilder()
            .State<State>(Idle)//전환 조건 미구현
                .Enter(state =>
                {
                    Debug.Log($"Entering {Idle} State");
                    //사거리 내에 들어올 때까지, 이동금지를 구현해야 합니다
                    SetIdle();
                })
                .Condition(() => 
                {
                    //감지 사거리 내에 들어옴을 판단합니다.
                    return false;
                },
                state =>
                {
                    //Walk state로 전환
                    state.Parent.ChangeState(Walk);
                })
                .End()
            .State<EnemyWalkState>(Walk)//걷는 것만 구현
                .Enter(state =>
                {
                    Debug.Log($"Entering {Walk} State");
                    //감지 사거리 내에 들어올 시, 걷기 상태로 진입합니다.
                    SetWalk();
                })
                .Condition(() =>
                {
                    //공격 사거리 내에 들어올 경우
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
            .State<State>(Run)//전환 조건 미정
                .Enter(state =>
                {
                    Debug.Log($"Entering {Run} State");
                    SetRun();
                })
                .End()
            .State<EnemyAttackState>(Attack)
                .Enter(state =>
                {
                    Debug.Log($"Entering {Attack} State");
                    state.AttackCount = stat.AttackDelay;//Attack 상태 진입시 공격을 유도합니다.
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
                    Debug.Log($"Entering {Damaged} State");
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
                    Debug.Log($"Entering {Die} State");
                    SetDie();
                })
                .End()
            .Build();
        rootState.ChangeState(Idle);
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
