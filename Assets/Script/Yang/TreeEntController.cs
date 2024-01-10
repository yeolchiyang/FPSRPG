using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

public class TreeEntController : Skeleton
{
    private const string Idle = "Idle";//bool
    private const string Walk = "Walk";//bool
    private const string Attack = "Attack";//Trigger
    private const string Enranged = "Enranged";//Trigger
    private const string Run = "Run";//bool
    //private const string Damaged = "Damaged";
    private const string Die = "Die";//bool

    //�������� ���θ� üũ�մϴ�.
    public bool IsInvulnerable { get; set; } = false;



    private void OnEnable()
    {
        //����Ʈ�� ���� �� �Ϲ� �� ������ �Ͻ����� ��
        //�����Ǿ��ִ� �Ϲ� ���� �Ҹ��ϴ� �Լ��� �����Ͽ�, OnEnable�� �߰��ؾ� �մϴ�.
        Respawn();
    }

    private void Start()
    {
        rootState = new StateMachineBuilder()
            .State<State>(Idle)//�⺻ �����Դϴ�. ������ ������ �������� �ʽ��ϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Idle} State");
                    //1.������Ʈ Ǯ�� ���� ������ �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Idle�� �����մϴ�.
                    ObjectSpawner.objectSpawner.StartSpawning();
                    SetBoolAnimation(Walk);
                })
                .Condition(() =>
                {
                    //�����Ÿ� ���� ������ ���
                    return IsTargetDetected();
                },
                state =>
                {
                    //Walk state�� ��ȯ
                    state.Parent.ChangeState(Walk);
                })
                .End()
            .State<State>(Walk)//�����Ÿ� ���� �� Walk�� �̵��մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Walk} State");
                    //1.������Ʈ Ǯ�� ������� �����ϴ� �Լ��� �����մϴ�.
                    //2.�ִϸ��̼� bool parameter�� Walk�� �����մϴ�.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(Walk);
                })
                .Update((state, deltaTime) =>
                {
                    
                })
                .End()
            .State<State>(Attack)//�����Ÿ� ���� ������ �Ϲ� �����մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Attack} State");
                    //�Ϲݰ��� �ִϸ��̼� 3���� �ݵ�� �Ͼ���� �����մϴ�.
                })
                .End()
            .State<State>(Enranged)//���� �� ���Ϸ� �������� �ߵ��ϴ� ����ȭ �Դϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Enranged} State");

                })
                .End()
            .State<State>(Run)//����ȭ ���¿��� �����Ÿ� ���� �� Run���� �̵��մϴ�.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Run} State");
                    
                })
                .End()
            .Build();
    }

    /// <summary>
    /// �¾��� ��, ����Ʈ�� �߻����� �ʽ��ϴ�.
    /// </summary>
    /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
    public override void SetDamaged(float damage)
    {
        if(IsInvulnerable)
        {
            return;
        }

        stat.CurrentHp -= damage;

        if( stat.CurrentHp / stat.MaxHp <= 0.3f )
        {
            //����ȭ�� �� �ִ� ���°����� �����մϴ�.
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
        if (IsInvulnerable)//�����ΰ�
        {
            return;
        }

        stat.CurrentHp -= damage;
        if (stat.CurrentHp / stat.MaxHp <= 0.3f)
        {
            //����ȭ�� �� �ִ� ���°����� �����մϴ�.
            rootState.ChangeState(Enranged);
        }

        if (stat.CurrentHp <= 0f)
        {
            //��� �� ó���� ������ ���� �޼ҵ带 ����ֽ��ϴ�.
        }
    }


    /// <summary>
    /// �Ϲݰ��� 3���� �����ϴ� ������ ������ �޼ҵ� �Դϴ�.
    /// </summary>
    private void IsAttacked()
    {
        if (IsTargetReached())
        {
            playerObject.GetComponent<Player_Health>().TakeDamage(stat.PhysicalDamage);
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
        if (distanceToPlayer <= stat.AttackRange)
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
    /// </summary>
    private void Respawn()
    {
        this.isActive = true;
        this.stat.CurrentHp = this.stat.MaxHp;
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
}
