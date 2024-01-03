using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yang{
    public class Skeleton : MonoBehaviour
    {
        private EnemyStat stat;
        private IState rootState;
        private UnityEngine.AI.NavMeshAgent skeletonNav;
        [SerializeField] private Animator skeletonAnimator;

        private const string Idle = "Idle";
        private const string Walk = "Walk";
        private const string Run = "Run";
        private const string Attack = "Attack";
        private const string Damaged = "Damaged";
        private const string Die = "Die";

        //Player�� singleton���� �����ϰ� �ִ� ��ü�� ���� ��� ��ü�� ��
        private GameObject playerObject;


        private void Awake()
        {
            skeletonNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            stat = GetComponent<EnemyStat>();
            playerObject = GameObject.FindWithTag("Player");
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
                .State<EnemyWalkState>(Walk)
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
                        state.ChangeState(Damaged);
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
                        if( state.AttackCount >= stat.AttackDelay )
                        {
                            state.AttackCount = 0f;
                            SetAttack();
                        }
                    })
                    .Condition(() =>
                    {
                        //Ÿ���� ���� ���, �������� ���¶� ��ȯ
                        return false;
                    },
                    state =>
                    {
                        //Damaged�� ��ȯ
                        Debug.Log($"{Damaged}�� ��ȯ");
                        state.Parent.ChangeState(Damaged);
                    })
                    .Condition(() => 
                    {
                        //�������� �ƴ� ���� & ��ȯ���� �ƴ� ���� & ��Ÿ��� ��� ���
                        bool isInTransition = skeletonAnimator.IsInTransition(0);
                        return !IsAttacking() && !IsTargetReached() && !isInTransition;
                    },
                    state =>
                    {
                        //Idle�� ��ȯ
                        Debug.Log($"{Walk}�� ��ȯ");
                        state.Parent.ChangeState(Walk);
                    })
                    .End()
                .State<State>(Damaged)
                    .Enter(state =>
                    {
                        Debug.Log($"Entering {Damaged} State");
                        SetDamaged(5);
                    })
                    .Condition(() =>
                    {
                        
                        return false;
                    },
                    state =>
                    {
                        //���� ���� true ��, ����� �ڵ� �ۼ�
                    })
                    .End()
                .State<State>(Die)
                    .Enter(state =>
                    {
                        Debug.Log($"Entering {Die} State");
                    })
                    .End()
                .Build();
            rootState.ChangeState(Walk);
            skeletonNav.stoppingDistance = stat.AttackRange;
        }

        private void FixedUpdate()
        {
            rootState.Update(Time.fixedDeltaTime);

        }


        private void SetIdle()
        {
            setBoolAnimation(Idle);
            StopNavigtaion();
        }
        
        private void SetWalk()
        {
            setBoolAnimation(Walk);
            StartNavigtaion(stat.WalkSpeed);
        }

        private void SetRun()
        {
            setBoolAnimation(Run);
            StartNavigtaion(stat.RunSpeed);
        }

        private void SetAttack()
        {
            SetTriggerAnimation(Attack);
            StopNavigtaion();
        }
        /// <summary>
        /// �ִϸ��̼� ���ڶ��� �������� ������
        /// </summary>
        /// <param name="physicalDamage">enemyStat�� physicalDamage�� �Ű������� �߰�</param>
        private void IsAttacked(int physicalDamage)
        {
            //�ӽ÷� �޼����� ����ϰ� ����
            Debug.Log($"Player���� {physicalDamage}Damage");
        }
        private bool IsAttacking()
        {
            AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
            bool isAttacking = currentState.IsName(Attack);
            Debug.Log("Attack ���ΰ�? : " + isAttacking);
            return isAttacking;
        }



        private void SetDamaged(float damage)
        {
            stat.Hp -= damage;
            SetTriggerAnimation(Damaged);
            StopNavigtaion();
        }

        private void SetDead()
        {
            SetTriggerAnimation(Die);
            StopNavigtaion();
        }

        //���� �̻��
        private void SetStun()
        {
            SetIdle();//Idle ��Ȱ��
            StopNavigtaion();
        }


        private void setBoolAnimation(string state)
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
        private string GetBoolAnimation()
        {

            return "";
        }

        private bool IsTargetReached()
        {
            bool isTargetReached = false;
            float distanceToPlayer = Vector3.Distance(
                            playerObject.transform.position, transform.position);
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


        private void StopNavigtaion()
        {
            if (!skeletonNav.isStopped)
            {
                skeletonNav.isStopped = true;
                skeletonNav.updatePosition = false;
                skeletonNav.updateRotation = false;
                skeletonNav.velocity = Vector3.zero;
            }
        }
        private void StartNavigtaion(float speed)
        {
            skeletonNav.isStopped = false;
            skeletonNav.ResetPath();//ResetPath -> SetDestination �ؾ� ���۵� �մϴ�.
            skeletonNav.SetDestination(playerObject.transform.position);
            skeletonNav.updatePosition = true;
            skeletonNav.updateRotation = true;
            skeletonNav.speed = speed;
        }


    }
}

