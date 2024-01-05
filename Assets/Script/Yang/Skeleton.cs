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

        //Player�� singleton���� �����ϰ� �ִ� ��ü�� ���� ��� ��ü�� ��
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
                        if( state.AttackCount >= stat.AttackDelay )
                        {
                            state.AttackCount = 0f;
                            SetAttack();
                        }
                    })
                    .Condition(() => 
                    {
                        //�������� �ƴ� ���� & ��ȯ���� �ƴ� ���� & ��Ÿ��� ��� ���
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
                        //�´� ��� ������ ���� �� �Ʒ��� ������ȯ
                        return !IsDamageAnimationPlaying();
                    },
                    state =>
                    {
                        //���� ���� true ��, ����� �ڵ� �ۼ�
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
        /// �ִϸ��̼� �߰��� �������� ������, �ִϸ��̼� Ŭ���� �ش� �Լ� ����
        /// </summary>
        /// <param name="physicalDamage">enemyStat�� physicalDamage�� �Ű������� �߰�</param>
        private void IsAttacked(int physicalDamage)
        {
            //�´� ���� ��Ÿ� ���� �־�߸� Damage
            if (IsTargetReached())
            {
                Debug.Log($"Player���� {physicalDamage}Damage");
            }
        }
        private bool IsAttackAnimationPlaying()
        {
            AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
            bool isAttacking = currentState.IsName(Attack);
            return isAttacking;
        }
        /// <summary>
        /// �¾��� ��, ����Ʈ�� �߻����� �ʽ��ϴ�.
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
        /// Raycast�� �̿��� ���� ��, ��ȣ�ۿ� ������ �޼ҵ� �Դϴ�.
        /// ���� ��ҿ� ����Ʈ�� ����ϴ�.
        /// </summary>
        /// <param name="hit">RaycastHit Object �־��ּ���</param>
        /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
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

        //��� ��, �ٸ� ������ȯ�� ���� �ʵ��� ó���ؾ� �մϴ�.
        
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
        /// ����ɱ� �����ϰ� 3�� ��, ObejctSpawner�� ���� ��ȯ �� ����
        /// ObjectPool�� ���ư���
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

        //���� �̻��
        private void SetStun()
        {
            SetIdle();//Idle ��Ȱ��
            StopNavigtaion();
        }


        private void SetBoolAnimation(string state)
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
        private string GetBoolAnimationName()
        {
            string parameterName = "";
            // ���� �ִϸ������� ��� �Ķ���͸� �����ɴϴ�.
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
        /// �����Ÿ� ���� �������� �����մϴ�.
        /// </summary>
        /// <returns>�����Ÿ��� ���� �� true</returns>
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

        
        private void OnCollisionEnter(Collision collision)
        {
            
        }

    }
}

