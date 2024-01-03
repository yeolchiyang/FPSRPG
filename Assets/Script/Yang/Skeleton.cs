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

        //Player를 singleton으로 저장하고 있는 객체가 있을 경우 대체할 것
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
                .State<State>(Idle)//전환 조건 미구현
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
                        state.ChangeState(Damaged);
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
                        if( state.AttackCount >= stat.AttackDelay )
                        {
                            state.AttackCount = 0f;
                            SetAttack();
                        }
                    })
                    .Condition(() =>
                    {
                        //타격을 받을 경우, 공격중인 상태라도 전환
                        return false;
                    },
                    state =>
                    {
                        //Damaged로 전환
                        Debug.Log($"{Damaged}로 전환");
                        state.Parent.ChangeState(Damaged);
                    })
                    .Condition(() => 
                    {
                        //공격중이 아닌 상태 & 전환중이 아닌 상태 & 사거리를 벗어날 경우
                        bool isInTransition = skeletonAnimator.IsInTransition(0);
                        return !IsAttacking() && !IsTargetReached() && !isInTransition;
                    },
                    state =>
                    {
                        //Idle로 전환
                        Debug.Log($"{Walk}로 전환");
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
        /// 애니메이션 끝자락에 가해지는 데미지
        /// </summary>
        /// <param name="physicalDamage">enemyStat의 physicalDamage를 매개변수로 추가</param>
        private void IsAttacked(int physicalDamage)
        {
            //임시로 메세지만 출력하게 구현
            Debug.Log($"Player에게 {physicalDamage}Damage");
        }
        private bool IsAttacking()
        {
            AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
            bool isAttacking = currentState.IsName(Attack);
            Debug.Log("Attack 중인가? : " + isAttacking);
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

        //현재 미사용
        private void SetStun()
        {
            SetIdle();//Idle 재활용
            StopNavigtaion();
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
        /// <summary>
        /// 공격, 데미지를 입었을 때 빠르게 기존 state로 전환하기 위함입니다.
        /// </summary>
        /// <returns>정의한 상태값의 string 값을 반환합니다.</returns>
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
            skeletonNav.ResetPath();//ResetPath -> SetDestination 해야 재작동 합니다.
            skeletonNav.SetDestination(playerObject.transform.position);
            skeletonNav.updatePosition = true;
            skeletonNav.updateRotation = true;
            skeletonNav.speed = speed;
        }


    }
}

