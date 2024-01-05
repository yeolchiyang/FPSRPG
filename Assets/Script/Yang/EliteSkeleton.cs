using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class EliteSkeleton : Skeleton
{
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
                    Debug.Log($"Entering {Idle} State");
                    SetIdle();
                })
                .End()
            .State<EnemyWalkState>(Walk)//걷는 것만 구현
                .Enter(state =>
                {
                    Debug.Log($"Entering {Walk} State");

                })
                .Update((state, deltaTime) =>
                {
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
            .State<State>(Run)//전환 조건 미구현
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

}
