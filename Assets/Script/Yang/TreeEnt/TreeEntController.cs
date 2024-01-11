using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public enum TreeState
{
    Idle,//bool
    Walk,//bool
    Attack,//Trigger
    Enranged,//Trigger
    EnrangedIdle,//bool
    Run,//bool
    Damaged,
    Die,//bool
    EnrangedAttack,//Trigger
    StompAttack,//Trigger
    JumpSmashAttack//Trigger
}

public class TreeEntController : Skeleton
{

    ///�������� ���θ� üũ�մϴ�.
    public bool IsInvulnerable { get; set; } = false;
    [Tooltip("���� ������ ���� Capsule Collider�� ����ֽ��ϴ�. " +
        "������� Collider�� ���̴� �⺻���� ��Ÿ��� 2�踸ŭ�� ũ�Ⱑ �˴ϴ�.")]
    [SerializeField] private CapsuleCollider[] AttackColliders;
    [SerializeField] public GameObject NormalHitEffect;
    public LayerMask playerMask;//�÷��̾� layer�� ���� �����Դϴ�.



    private void OnEnable()
    {
        //����Ʈ�� ���� �� �Ϲ� �� ������ �Ͻ����� ��
        //�����Ǿ��ִ� �Ϲ� ���� �Ҹ��ϴ� �Լ��� �����Ͽ�, OnEnable�� �߰��ؾ� �մϴ�.
        Respawn();
        //transform.parent.SendMessage()
    }

    private void Start()
    {
        rootState = new StateMachineBuilder()
            .State<State>(TreeState.Idle.ToString())//�⺻ �����Դϴ�. ������ ������ �������� �ʽ��ϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Idle.ToString()} State");
                    //1.������Ʈ Ǯ�� ���� ������ �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Idle�� �����մϴ�.
                    //3.�÷��̾� ������ �����մϴ�.(NavMesh)
                    ObjectSpawner.objectSpawner.StartSpawning();
                    SetBoolAnimation(TreeState.Idle.ToString());
                    StopNavigtaion();
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ������ ���
                    return IsTargetDetected();
                },
                state =>
                {
                    //Walk state�� ��ȯ
                    state.Parent.ChangeState(TreeState.Walk.ToString());
                })
                .End()
            .State<TreeEntWalkState>(TreeState.Walk.ToString())//�����Ÿ� ���� �� Walk�� �̵��մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Walk.ToString()} State");
                    //1.������Ʈ Ǯ�� ������� �����ϴ� �Լ��� �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Walk�� �����մϴ�.
                    //3.�÷��̾ �����մϴ�.(NavMesh)
                    //4.���ݿ� Collider�� ��Ȱ��ȭ �մϴ�.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(TreeState.Walk.ToString());
                    StartNavigtaion(stat.WalkSpeed);
                    ToggleCollider();
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ����� ���
                    return !IsTargetDetected();
                },
                state =>
                {
                    //Idle state�� ��ȯ
                    state.Parent.ChangeState(TreeState.Idle.ToString());
                })
                .Condition(() =>
                {
                    //���� ��Ÿ� ���� ������ ���
                    return IsTargetReached();
                },
                state =>
                {
                    //���� ��Ÿ���� ������ ��쿡�� Attack state�� ��ȯ�մϴ�.
                    if ((state.AttackedTime + stat.AttackDelay) <= Time.time)
                    {
                        state.Parent.ChangeState(TreeState.Attack.ToString());
                        state.AttackedTime = Time.time;
                    }
                })
                .End()
            .State<State>(TreeState.Attack.ToString())//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Attack.ToString()} State");
                    //1.�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                    //2.NavMesh�� �����մϴ�.
                    //3.���ݿ� Collider�� Ȱ��ȭ �մϴ�.
                    SetTriggerAnimation(TreeState.Attack.ToString());
                    StopNavigtaion();
                    ToggleCollider();
                })
                .End()
            .State<State>(TreeState.Enranged.ToString())//���� �� ���Ϸ� �������� �ߵ��ϴ� ����ȭ �Դϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Enranged.ToString()} State");
                    SetTriggerAnimation(TreeState.Enranged.ToString());
                    StopNavigtaion();
                    //����ȭ ��� ��, Idle�� ���ư��ϴ�.
                    //state.Parent.ChangeState(TreeState.EnrangedIdle.ToString());
                })
                .End()
            .State<State>(TreeState.EnrangedIdle.ToString())//����ȭ ������ Idle�Դϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.EnrangedIdle.ToString()} State");
                    SetBoolAnimation(TreeState.EnrangedIdle.ToString());
                    StopNavigtaion();
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ������ ���
                    return IsTargetDetected();
                },
                state =>
                {
                    //Run state�� ��ȯ
                    state.Parent.ChangeState(TreeState.Run.ToString());
                })
                .End()
            .State<TreeEntWalkState>(TreeState.Run.ToString())//����ȭ ���¿��� �����Ÿ� ���� �� Run���� �̵��մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Run.ToString()} State");
                    //1.������Ʈ Ǯ�� ������� �����ϴ� �Լ��� �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Walk�� �����մϴ�.
                    //3.�÷��̾ �����մϴ�.(NavMesh)
                    //4.���ݿ� Collider�� ��Ȱ��ȭ �մϴ�.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(TreeState.Run.ToString());
                    StartNavigtaion(stat.RunSpeed);
                    ToggleCollider();
                })
                .Condition(() =>
                {
                    //���� ��Ÿ� ���� ������ ���
                    return IsTargetReached();
                },
                state =>
                {
                    //���� ��Ÿ���� ������ ��쿡�� Attack state�� ��ȯ�մϴ�.
                    if ((state.AttackedTime + stat.AttackDelay) <= Time.time)
                    {
                        state.Parent.ChangeState(TreeState.EnrangedAttack.ToString());
                        state.AttackedTime = Time.time;
                    }
                })
                .End()
            .State<State>(TreeState.EnrangedAttack.ToString())//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.EnrangedAttack.ToString()} State");
                    //�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                    //NavMesh�� �����մϴ�.
                    //���ݿ� Collider�� Ȱ��ȭ �մϴ�.
                    SetTriggerAnimation(TreeState.EnrangedAttack.ToString());
                    StopNavigtaion();
                })
                .End()
            .Build();

        rootState.ChangeState(TreeState.Idle.ToString());//�ʱ� ���� Idle
    }

    /// <summary>
    /// �¾��� ��, ����Ʈ�� �߻����� �ʽ��ϴ�.
    /// </summary>
    /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
    public override void SetDamaged(float damage)
    {
        if(IsInvulnerable)
        {
            Debug.Log("�����Դϴ�.");
            return;
        }
        Debug.Log("damage����");
        stat.CurrentHp -= damage;

        if( stat.CurrentHp / stat.MaxHp <= 0.3f )
        {
            //����ȭ�� �� �ִ� ���°����� �����մϴ�.
            IsInvulnerable = true;//����
            rootState.ChangeState(TreeState.Enranged.ToString());
        }

        if (stat.CurrentHp <= 0f)
        {
            //��� �� ó���� ������ ���� �޼ҵ带 ����ֽ��ϴ�.
        }

    }
    /// <summary>
    /// Raycast�� �̿��� ���� ��, ��ȣ�ۿ� ������ �޼ҵ� �Դϴ�.
    /// ���� ��ҿ� ����Ʈ�� ����ϴ�.
    /// </summary>
    /// <param name="hit">RaycastHit Object �־��ּ���</param>
    /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
    public override void SetDamaged(RaycastHit hit, float damage)
    {
        if (IsInvulnerable)//��������
        {
            return;
        }

        stat.CurrentHp -= damage;
        if (stat.CurrentHp / stat.MaxHp <= 0.3f)
        {
            //����ȭ�� �� �ִ� ���°����� �����մϴ�.
            rootState.ChangeState(TreeState.Enranged.ToString());
        }

        if (stat.CurrentHp <= 0f)
        {
            //��� �� ó���� ������ ���� �޼ҵ带 ����ֽ��ϴ�.
        }
    }


    /// <summary>
    /// �Ϲݰ��� 3���� �����ϴ� ������ ������ �޼ҵ� �Դϴ�.
    /// </summary>
    public void IsAttacked()
    {
        playerObject.GetComponent<Player_Health>().TakeDamage(stat.PhysicalDamage);

    }
    /// <summary>
    /// �ȿ� �޸� �� Collider�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �޼ҵ� �Դϴ�.
    /// Idle State ��, Collider�� ��Ȱ��ȭ �մϴ�. 
    /// Attack State ��, Collider�� Ȱ��ȭ �մϴ�.
    /// </summary>
    private void ToggleCollider()
    {
        foreach (Collider attackCollider in AttackColliders)
        {
            attackCollider.enabled = !attackCollider.enabled;
        }
    }

    /// <summary>
    /// �����Ÿ� ���� �������� �Ǵ��մϴ�.
    /// </summary>
    /// <returns>�����Ÿ� ���� ���� �� true</returns>
    private bool IsTargetDetected()
    {
        bool isTargetDetected = false;
        Vector3 enemyXZ = new Vector3(transform.position.x,
                                        0, transform.position.z);
        Vector3 playerXZ = new Vector3(playerObject.transform.position.x,
                                        0, playerObject.transform.position.z);
        float distanceToPlayer = Vector3.Distance(
                        playerXZ, enemyXZ);
        if (distanceToPlayer <= stat.DetectionRange)
        {
            isTargetDetected = true;
        }
        return isTargetDetected;
    }


    /// <summary>
    /// �����Ÿ� ���� �������� �����մϴ�.
    /// ������ ���� ���� �ʿ��� �� �����մϴ�
    /// </summary>
    /// <returns>�����Ÿ��� ���� �� true</returns>
    private bool IsTargetReached()
    {
        bool isTargetReached = false;
        Vector3 enemyXZ = new Vector3(transform.position.x,
                                0, transform.position.z);
        Vector3 playerXZ = new Vector3(playerObject.transform.position.x,
                                        0, playerObject.transform.position.z);
        float distanceToPlayer = Vector3.Distance(
                        playerXZ, enemyXZ);
        if (distanceToPlayer <= stat.AttackRange)
        {
            isTargetReached = true;
        }
        return isTargetReached;
    }


    /// <summary>
    /// ���� ��ȯ�� �� ����˴ϴ�.
    /// ���� ��Ÿ� * 2 ��ŭ capsule collider ũ�⸦ �����մϴ�.
    /// Active �Ӽ� : ����ִ����� �Ǻ��մϴ�.
    /// </summary>
    private void Respawn()
    {
        this.isActive = true;
        this.stat.CurrentHp = this.stat.MaxHp;
        foreach(CapsuleCollider capsule in AttackColliders)
        {
            capsule.height = stat.AttackRange * 2;
        }
        playerMask = playerObject.layer;
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
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                //true�� �Ķ���͸�
                if (skeletonAnimator.GetBool(parameter.name))
                {
                    parameterName = parameter.name;
                }
            }

        }
        return parameterName;
    }

    private void SetTriggerAnimation(string state)
    {
        skeletonAnimator.SetTrigger(state);
    }

}
