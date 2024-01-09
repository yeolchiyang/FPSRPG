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

    //무적상태 여부를 체크합니다.
    public bool IsInvulnerable { get; set; } = false;



    private void OnEnable()
    {
        //엘리트몹 등장 시 일반 몹 리스폰 일시정지 및
        //생성되어있는 일반 몹이 소멸하는 함수를 구현하여, OnEnable에 추가해야 합니다.
        Respawn();
    }

    private void Start()
    {
        rootState = new StateMachineBuilder()
            .State<State>(Idle)//기본 상태입니다. 감지할 때까지 움직이지 않습니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Idle} State");
                    //1.오브젝트 풀의 유닛 생성을 시작합니다.
                    //2.애니메이션 bool parameter를 Idle로 변경합니다.
                    ObjectSpawner.objectSpawner.StartSpawning();
                    SetBoolAnimation(Walk);
                })
                .Condition(() =>
                {
                    //감지거리 내에 들어왔을 경우
                    return IsTargetDetected();
                },
                state =>
                {
                    //Walk state로 전환
                    state.Parent.ChangeState(Walk);
                })
                .End()
            .State<State>(Walk)//사정거리 밖일 시 Walk로 이동합니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Walk} State");
                    //1.오브젝트 풀의 재생성을 중지하는 함수를 실행합니다.
                    //2.애니메이션 bool parameter를 Walk로 변경합니다.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(Walk);
                })
                .Update((state, deltaTime) =>
                {
                    
                })
                .End()
            .State<State>(Attack)//사정거리 내에 들어오면 일반 공격합니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Attack} State");
                    //일반공격 애니메이션 3번이 반드시 일어나도록 구현합니다.
                })
                .End()
            .State<State>(Enranged)//일정 피 이하로 내려가면 발동하는 광폭화 입니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Enranged} State");

                })
                .End()
            .State<State>(Run)//광폭화 상태에만 사정거리 밖일 시 Run으로 이동합니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {Run} State");
                    
                })
                .End()
            .Build();
    }

    /// <summary>
    /// 맞았을 때, 이펙트가 발생되지 않습니다.
    /// </summary>
    /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
    public override void SetDamaged(float damage)
    {
        if(IsInvulnerable)
        {
            return;
        }

        stat.CurrentHp -= damage;

        if( stat.CurrentHp / stat.MaxHp <= 0.3f )
        {
            //광폭화될 수 있는 상태값으로 변경합니다.
        }

        if (stat.CurrentHp <= 0f)
        {
            //사망 시 처리할 로직을 담은 메소드를 집어넣습니다.
        }

    }
    /// <summary>
    /// Raycast를 이용한 공격 시, 상호작용 가능한 메소드 입니다.
    /// 맞은 장소에 이펙트가 생깁니다.
    /// </summary>
    /// <param name="hit">RaycastHit Object 넣어주세요</param>
    /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
    public override void SetDamaged(RaycastHit hit, float damage)
    {
        if (IsInvulnerable)//무적인가
        {
            return;
        }

        stat.CurrentHp -= damage;
        if (stat.CurrentHp / stat.MaxHp <= 0.3f)
        {
            //광폭화될 수 있는 상태값으로 변경합니다.
            rootState.ChangeState(Enranged);
        }

        if (stat.CurrentHp <= 0f)
        {
            //사망 시 처리할 로직을 담은 메소드를 집어넣습니다.
        }
    }


    /// <summary>
    /// 일반공격 3개가 공유하는 데미지 입히는 메소드 입니다.
    /// </summary>
    private void IsAttacked()
    {
        if (IsTargetReached())
        {
            playerObject.GetComponent<Player_Health>().TakeDamage(stat.PhysicalDamage);
        }
    }

    /// <summary>
    /// 감지거리 내에 들어온지를 판단합니다.
    /// </summary>
    /// <returns>감지거리 내에 들어올 시 true</returns>
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
    /// 사정거리 내에 들어온지를 감지합니다.
    /// 판정에 대한 수정 필요할 시 수정합니다
    /// </summary>
    /// <returns>사정거리에 들어올 시 true</returns>
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
    /// 몹이 소환될 때 실행됩니다.
    /// </summary>
    private void Respawn()
    {
        this.isActive = true;
        this.stat.CurrentHp = this.stat.MaxHp;
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
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                //true인 파라미터면
                if (skeletonAnimator.GetBool(parameter.name))
                {
                    parameterName = parameter.name;
                }
            }

        }
        return parameterName;
    }
}
