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
            .State<State>(Idle)//��ȯ ���� �̱���
                .Enter(state =>
                {
                    Debug.Log($"Entering {Idle} State");
                    SetIdle();
                })
                .End()
            .State<EnemyWalkState>(Walk)//�ȴ� �͸� ����
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
            .State<State>(Run)//��ȯ ���� �̱���
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
                    Debug.Log($"Entering {Damaged} State");
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
