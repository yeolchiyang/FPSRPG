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

        //Player�� singleton���� �����ϰ� �ִ� ��ü�� ���� ��� ��ü�� ��
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
        /// �ִϸ��̼� �߰��� �������� ������, �ִϸ��̼� Ŭ���� �ش� �Լ� ����
        /// </summary>
        /// <param name="physicalDamage">enemyStat�� physicalDamage�� �Ű������� �߰�</param>
        protected void IsAttacked()
        {
            //�´� ���� ��Ÿ� ���� �־�߸� Damage
            if (IsTargetReached())
            {
                playerObject.GetComponent<Player_Health>().TakeDamage(stat.PhysicalDamage);
            }
        }
        /// <summary>
        /// �¾��� ��, ����Ʈ�� �߻����� �ʽ��ϴ�.
        /// </summary>
        /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
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
        /// Raycast�� �̿��� ���� ��, ��ȣ�ۿ� ������ �޼ҵ� �Դϴ�.
        /// ���� ��ҿ� ����Ʈ�� ����ϴ�.
        /// </summary>
        /// <param name="hit">RaycastHit Object �־��ּ���</param>
        /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
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

        //��� ��, �ٸ� ������ȯ�� ���� �ʵ��� ó���ؾ� �մϴ�.
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
        /// ����ɱ� �����ϰ� 3�� ��, ObejctSpawner�� ���� ��ȯ �� ����
        /// ObjectPool�� ���ư���
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

        //���� �̻��
        protected void SetStun()
        {
            SetIdle();//Idle ��Ȱ��
            StopNavigtaion();
        }


        protected void SetBoolAnimation(string state)
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
        protected string GetBoolAnimationName()
        {
            string parameterName = "";
            // ���� �ִϸ������� ��� �Ķ���͸� �����ɴϴ�.
            AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                if(parameter.type == AnimatorControllerParameterType.Bool)
                {
                    //true�� �Ķ���͸�
                    if(skeletonAnimator.GetBool(parameter.name))
                    {
                        parameterName = parameter.name;
                    }
                }

            }
            return parameterName;
        }
        /// <summary>
        /// �����Ÿ� ���� �������� �����մϴ�.
        /// </summary>
        /// <returns>�����Ÿ��� ���� �� true</returns>
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
            skeletonNav.ResetPath();//ResetPath -> SetDestination �ؾ� ���۵� �մϴ�.
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

