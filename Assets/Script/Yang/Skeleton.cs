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

        //임시, 테스트 후 삭제
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
                        //공격 딜레이에 맞춰 공격하는 것을 구현
                    })
                    .Condition(() => 
                    {
                        //Condition이 반환하는 bool값에 따라, 아래의 코드가 실행될 지 결정
                        return false;
                    },
                    state =>
                    {
                        //위의 조건 true 시, 실행될 코드 작성
                    })
                    .End()
                .State<State>(Damaged)
                    .Enter(state =>
                    {
                        Debug.Log($"Entering {Damaged} State");
                    })
                    .Condition(() =>
                    {
                        //Condition이 반환하는 bool값에 따라, 아래의 코드가 실행될 지 결정
                        return false;
                    },
                    state =>
                    {
                        //위의 조건 true 시, 실행될 코드 작성
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

        //현재 미사용
        private void SetStun()
        {
            SetIdle();//Idle 재활용
            stopNavigtaion();
        }


        private void setBoolAnimation(string state)
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
        private void setTriggerAnimation(string state)
        {
            skeletonAnimator.SetTrigger(state);
        }

        /// <summary>
        /// 네비게이션 멈추기
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

