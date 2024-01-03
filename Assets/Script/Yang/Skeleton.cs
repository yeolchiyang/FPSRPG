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

        //�ӽ�, �׽�Ʈ �� ����
        [SerializeField] private Transform targetPoint;
        

        private void Awake()
        {
            skeletonNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            stat = GetComponent<EnemyStat>();
        }

        private void Start()
        {
            rootState = new StateMachineBuilder()
                .State<State>(Idle)
                    .Enter(state =>
                    {
                        Debug.Log($"Entering {Idle} State");
                        SetIdle();
                    })
                    .End()
                .State<State>(Walk)
                    .Enter(state =>
                    {
                        Debug.Log($"Entering {Walk} State");
                        SetWalk();
                    })
                    .End()
                .State<State>(Run)
                    .Enter(state =>
                    {
                        Debug.Log($"Entering {Run} State");
                        SetRun();
                    })
                    .End()
                .State<State>(Attack)
                    .Enter(state =>
                    {
                        Debug.Log($"Entering {Attack} State");
                    })
                    .Update((state, deltaTime) =>
                    {
                        //���� �����̿� ���� �����ϴ� ���� ����
                    })
                    .Condition(() => 
                    {
                        //Condition�� ��ȯ�ϴ� bool���� ����, �Ʒ��� �ڵ尡 ����� �� ����
                        return false;
                    },
                    state =>
                    {
                        //���� ���� true ��, ����� �ڵ� �ۼ�
                    })
                    .End()
                .State<State>(Damaged)
                    .Enter(state =>
                    {
                        Debug.Log($"Entering {Damaged} State");
                    })
                    .Condition(() =>
                    {
                        //Condition�� ��ȯ�ϴ� bool���� ����, �Ʒ��� �ڵ尡 ����� �� ����
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
            skeletonNav.SetDestination(targetPoint.position);
        }

        private void FixedUpdate()
        {
            rootState.Update(Time.fixedDeltaTime);
        }


        private void SetIdle()
        {
            setBoolAnimation(Idle);
            stopNavigtaion();
        }
        
        private void SetWalk()
        {
            setBoolAnimation(Walk);
            startNavigtaion(stat.WalkSpeed);
        }

        private void SetRun()
        {
            setBoolAnimation(Run);
            startNavigtaion(stat.RunSpeed);
        }

        private void SetAttack()
        {
            setTriggerAnimation(Attack);
            stopNavigtaion();

        }
        private void IsAttacked()
        {

        }

        private void SetDamaged(float damage)
        {
            stat.Hp -= damage;
            setTriggerAnimation(Damaged);
            stopNavigtaion();
        }

        private void SetDead()
        {
            setTriggerAnimation(Die);
            stopNavigtaion();
        }

        //���� �̻��
        private void SetStun()
        {
            SetIdle();//Idle ��Ȱ��
            stopNavigtaion();
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
        private void setTriggerAnimation(string state)
        {
            skeletonAnimator.SetTrigger(state);
        }

        /// <summary>
        /// �׺���̼� ���߱�
        /// </summary>
        private void stopNavigtaion()
        {
            if (!skeletonNav.isStopped)
            {
                skeletonNav.isStopped = true;
                skeletonNav.velocity = new Vector3(0f, 0f, 0f);
            }
        }
        private void startNavigtaion(float speed)
        {
            skeletonNav.speed = stat.RunSpeed;
            skeletonNav.isStopped = false;
        }


    }
}

