using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yang{
    public class Skeleton : MonoBehaviour
    {
        private bool isActive = true;
        private EnemyStat stat;
        public IState rootState;
        private UnityEngine.AI.NavMeshAgent skeletonNav;
        [SerializeField] private Animator skeletonAnimator;
        [SerializeField] private GameObject skeletonHitEffect;

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
                        if( state.AttackCount >= stat.AttackDelay )
                        {
                            state.AttackCount = 0f;
                            SetAttack();
                        }
                    })
                    .Condition(() => 
                    {
                        //공격중이 아닌 상태 & 전환중이 아닌 상태 & 사거리를 벗어날 경우
                        bool isInTransition = skeletonAnimator.IsInTransition(0);
                        return !IsAttackAnimationPlaying() && 
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
                        SetDamaged(5);
                    })
                    .Condition(() =>
                    {
                        //맞는 모션 완전히 종료 시 아래로 상태전환
                        return !IsDamageAnimationPlaying();
                    },
                    state =>
                    {
                        //위의 조건 true 시, 실행될 코드 작성
                        string stateName = GetBoolAnimationName();
                        state.Parent.ChangeState(stateName);
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
        private void IsAttacked(int physicalDamage)
        {
            //맞는 순간 사거리 내에 있어야만 Damage
            if (IsTargetReached())
            {
                Debug.Log($"Player에게 {physicalDamage}Damage");
            }
        }
        private bool IsAttackAnimationPlaying()
        {
            AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
            bool isAttacking = currentState.IsName(Attack);
            return isAttacking;
        }
        /// <summary>
        /// 맞았을 때, 이펙트가 발생되지 않습니다.
        /// </summary>
        /// <param name="damage"></param>
        public void SetDamaged(float damage)
        {
            stat.CurrentHp -= damage;
            SetTriggerAnimation(Damaged);
            StopNavigtaion();
            if(stat.CurrentHp <= 0f)
            {
                rootState.ChangeState(Die);
            }
        }
        /// <summary>
        /// Raycast를 이용한 공격 시, 상호작용 가능한 메소드 입니다.
        /// 맞은 장소에 이펙트가 생깁니다.
        /// </summary>
        /// <param name="hit">RaycastHit Object 넣어주세요</param>
        /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
        public void SetDamaged(RaycastHit hit, float damage)
        {
            stat.CurrentHp -= damage;
            SetTriggerAnimation(Damaged);
            StopNavigtaion();
            GameObject spawnedDecal = Instantiate(
               skeletonHitEffect, hit.point, 
               Quaternion.LookRotation(hit.normal));
            if (stat.CurrentHp <= 0f)
            {
                rootState.ChangeState(Die);
            }
        }

        private bool IsDamageAnimationPlaying()
        {
            AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
            bool isDamageAnimationPlaying = currentState.IsName(Attack);
            return isDamageAnimationPlaying;
        }

        //사망 시, 다른 상태전환이 되지 않도록 처리해야 합니다.
        
        private void SetDie()
        {
            this.isActive = false;
            SetTriggerAnimation(Die);
            StopNavigtaion();
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            StartCoroutine(Sinking());
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
            while (true)
            {
                transform.position += Vector3.down * Time.fixedDeltaTime * 0.5f;
                timer += Time.deltaTime;
                if(timer > 3f)
                {
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
                if(parameter.type == AnimatorControllerParameterType.Bool)
                {
                    if(parameter.defaultBool == true)
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

        
        private void OnCollisionEnter(Collision collision)
        {
            
        }

    }
}

