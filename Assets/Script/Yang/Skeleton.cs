using RSG;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

namespace Yang{
    public abstract class Skeleton : MonoBehaviour
    {
        protected bool isActive = true;
        protected EnemyStat stat;
        public IState rootState;
        protected UnityEngine.AI.NavMeshAgent skeletonNav;
        [SerializeField] protected Animator skeletonAnimator;
        [SerializeField] protected GameObject skeletonHitEffect;

        protected const string Idle = "Idle";
        protected const string Walk = "Walk";
        protected const string Run = "Run";
        protected const string Attack = "Attack";
        protected const string Damaged = "Damaged";
        protected const string Die = "Die";

        //Player를 singleton으로 저장하고 있는 객체가 있을 경우 대체할 것
        protected GameObject playerObject;

        protected void Awake()
        {
            skeletonNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            stat = GetComponent<EnemyStat>();
            playerObject = GameObject.FindWithTag("Player");
        }

        protected void SetIdle()
        {
            SetBoolAnimation(Idle);
            StopNavigtaion();
        }

        protected void SetWalk()
        {
            SetBoolAnimation(Walk);
            StartNavigtaion(stat.WalkSpeed);
        }

        protected void SetRun()
        {
            SetBoolAnimation(Run);
            StartNavigtaion(stat.RunSpeed);
        }

        protected void SetAttack()
        {
            SetTriggerAnimation(Attack);
            StopNavigtaion();
        }
        /// <summary>
        /// 애니메이션 중간에 가해지는 데미지, 애니메이션 클립에 해당 함수 존재
        /// </summary>
        /// <param name="physicalDamage">enemyStat의 physicalDamage를 매개변수로 추가</param>
        protected void IsAttacked()
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
        public virtual void SetDamaged(float damage)
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
        /// 맞은 장소에 이펙트가 생깁니다.
        /// </summary>
        /// <param name="hit">RaycastHit Object 넣어주세요</param>
        /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
        public virtual void SetDamaged(RaycastHit hit, float damage)
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
                GameObject hitEffect = Instantiate(
                   skeletonHitEffect, hit.point,
                   Quaternion.LookRotation(hit.normal));
                rootState.TriggerEvent(Damaged);
            }
        }

        protected bool IsAnimationPlaying(string stateName)
        {
            AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
            bool isAnimationPlaying = currentState.IsName(stateName);
            return isAnimationPlaying;
        }

        //사망 시, 다른 상태전환이 되지 않도록 처리해야 합니다.
        protected void SetDie()
        {
            this.isActive = false;
            if (!IsAnimationPlaying(Die))
            {
                SetTriggerAnimation(Die);
                StopNavigtaion();
                StartCoroutine(Sinking());
            }
        }
        /// <summary>
        /// 가라앉기 시작하고 3초 후, ObejctSpawner의 현재 소환 수 감소
        /// ObjectPool로 돌아가기
        /// </summary>
        /// <returns></returns>
        protected IEnumerator Sinking()
        {
            yield return new WaitForSeconds(1f);
            float timer = 0f;
            float sinkingStartTime = 3.4f;
            float destroyTime = 5f;
            while (true)
            {
                timer += Time.deltaTime;
                if(timer > sinkingStartTime)
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

        protected void Respawn()
        {
            this.isActive = true;
            this.stat.CurrentHp = this.stat.MaxHp;
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;

        }

        //현재 미사용
        protected void SetStun()
        {
            SetIdle();//Idle 재활용
            StopNavigtaion();
        }


        protected void SetBoolAnimation(string state)
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
        protected string GetBoolAnimationName()
        {
            string parameterName = "";
            // 현재 애니메이터의 모든 파라미터를 가져옵니다.
            AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                if(parameter.type == AnimatorControllerParameterType.Bool)
                {
                    //true인 파라미터면
                    if(skeletonAnimator.GetBool(parameter.name))
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
        protected bool IsTargetReached()
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

        protected void SetTriggerAnimation(string state)
        {
            skeletonAnimator.SetTrigger(state);
        }


        protected void StopNavigtaion()
        {
            if (!skeletonNav.isStopped)
            {
                skeletonNav.isStopped = true;
                skeletonNav.updatePosition = false;
                skeletonNav.updateRotation = false;
                skeletonNav.velocity = Vector3.zero;
            }
        }
        protected void StartNavigtaion(float speed)
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

