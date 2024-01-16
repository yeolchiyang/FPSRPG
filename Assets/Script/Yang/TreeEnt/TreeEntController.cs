using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    Die,//Trigger
    EnrangedAttack,//Trigger
    StompAttack,//Trigger
    JumpSmashAttack//Trigger
}

public class TreeEntController : Skeleton
{

    ///무적상태 여부를 체크합니다.
    public bool IsInvulnerable { get; set; } = false;
    /// <summary>
    /// 분노상태 여부를 저장합니다.
    /// </summary>
    private bool isEnranged = false;
    [Tooltip("공격 범위를 가진 Capsule Collider를 집어넣습니다. " +
        "집어넣은 Collider의 높이는 기본공격 사거리의 2배만큼의 크기가 됩니다.")]
    [SerializeField] private CapsuleCollider[] AttackColliders;
    [Tooltip("Player에게 데미지를 입힐 시 충돌이 일어난 좌표에 생성되는 이펙트입니다")]
    [SerializeField] public GameObject NormalHitEffect;
    [Tooltip("TreeEnt가 벗어나면 안되는 영역을 Trigger로 표시한 object 입니다.")]
    [SerializeField] private BoxCollider TreeEntBoxCollider;

    public LayerMask playerMask;//플레이어 layer를 담은 변수입니다.
    


    private void OnEnable()
    {
        //엘리트몹 등장 시 일반 몹 리스폰 일시정지 및
        //생성되어있는 일반 몹이 소멸하는 함수를 구현하여, OnEnable에 추가해야 합니다.
        Respawn();
        //transform.parent.SendMessage()
    }

    private void Start()
    {
        rootState = new StateMachineBuilder()
            .State<State>(TreeState.Idle.ToString())//기본 상태입니다. 감지할 때까지 움직이지 않습니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Idle.ToString()} State");
                    //1.오브젝트 풀의 유닛 생성을 시작합니다.
                    //2.애니메이션 bool parameter를 Idle로 변경합니다.
                    //3.플레이어 추적을 중지합니다.(NavMesh)
                    ObjectSpawner.objectSpawner.StartSpawning();
                    SetBoolAnimation(TreeState.Idle.ToString());
                    StopNavigtaion();
                })
                .Condition(() =>
                {
                    //감지거리 내에 들어왔을 경우
                    return IsTargetDetected();
                },
                state =>
                {
                    //Walk state로 전환
                    state.Parent.ChangeState(TreeState.Walk.ToString());
                })
                .End()
            .State<TreeEntWalkState>(TreeState.Walk.ToString())//사정거리 밖일 시 Walk로 이동합니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Walk.ToString()} State");
                    //1.오브젝트 풀의 재생성을 중지하는 함수를 실행합니다.
                    //2.애니메이션 bool parameter를 Walk로 변경합니다.
                    //3.플레이어를 추적합니다.(NavMesh)
                    //4.공격용 Collider를 비활성화 합니다.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(TreeState.Walk.ToString());
                    StartNavigtaion(stat.WalkSpeed);
                    ToggleAttackCollider(false);
                })
                .Condition(() =>
                {
                    //감지거리 내이며, Trigger에 감지되지 않았을 시 true
                    return IsTargetDetected() && !IsOutsideBounds();
                },
                state =>
                {
                    //갱신 주기마다, 목적지가 초기화 됩니다.
                    if ((state.LastResetDestinationTime + stat.ResetDestinationDelay) <= Time.time)
                    {
                        StartNavigtaion(stat.WalkSpeed);
                        state.LastResetDestinationTime = Time.time;
                    }
                })
                .Condition(() =>
                {
                    //Trigger에 감지되었으면 일단 true를 반환 
                    return IsOutsideBounds();
                },
                state =>
                {
                    /* must======
                     * 1. Trigger에 감지되고, 플레이어가 감지거리 밖인 경우 ->
                     * 돌아가기(navMesh의 destination 변경)
                     * 2. Trigger에 감지되고, 플레이어가 감지거리 내인 경우 -> 
                     * 계속 Walk, StopNavigation
                     */
                    if (!IsTargetDetected()) //플레이어가 감지거리 밖인 경우
                    {
                        StartNavigation(stat.WalkSpeed, TreeEntBoxCollider.transform.position);
                    }
                    else
                    {
                        StopNavigtaion();
                    }
                })
                .Condition(() =>
                {
                    //navmesh 목적지가 중앙이며, 중앙에 근접한 경우 true
                    return IsEntInCenter();
                },
                state =>
                {
                    state.Parent.ChangeState(TreeState.Idle.ToString());
                })
                .Condition(() =>
                {
                    //공격 사거리 내로 들어왔을 경우
                    return IsTargetReached();
                },
                state =>
                {
                    //공격 쿨타임이 돌았을 경우에만 Attack state로 전환합니다.
                    if ((state.AttackedTime + stat.AttackDelay) <= Time.time)
                    {
                        state.Parent.ChangeState(TreeState.Attack.ToString());
                        state.AttackedTime = Time.time;
                    }
                })
                .End()
            .State<State>(TreeState.Attack.ToString())//사정거리 내에 들어오면 일반 공격합니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Attack.ToString()} State");
                    //1.일반공격 애니메이션 3번이 반드시 일어나도록 구현합니다.
                    //2.NavMesh를 중지합니다.
                    //3.공격용 Collider를 활성화 합니다.
                    SetTriggerAnimation(TreeState.Attack.ToString());
                    StopNavigtaion();
                    ToggleAttackCollider(true);
                })
                .End()
            .State<State>(TreeState.Enranged.ToString())//일정 피 이하로 내려가면 발동하는 광폭화 입니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Enranged.ToString()} State");
                    //광폭화 모션 후, Idle로 돌아갑니다.
                    SetTriggerAnimation(TreeState.Enranged.ToString());
                    StopNavigtaion();
                })
                .End()
            .State<State>(TreeState.EnrangedIdle.ToString())//광폭화 상태의 Idle입니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.EnrangedIdle.ToString()} State");
                    SetBoolAnimation(TreeState.EnrangedIdle.ToString());
                    StopNavigtaion();
                })
                .Condition(() =>
                {
                    //감지거리 내에 들어왔을 경우
                    return IsTargetDetected();
                },
                state =>
                {
                    //Run state로 전환
                    state.Parent.ChangeState(TreeState.Run.ToString());
                })
                .End()
            .State<TreeEntWalkState>(TreeState.Run.ToString())//광폭화 상태에만 사정거리 밖일 시 Run으로 이동합니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Run.ToString()} State");
                    //1.오브젝트 풀의 재생성을 중지하는 함수를 실행합니다.
                    //2.애니메이션 bool parameter를 Walk로 변경합니다.
                    //3.플레이어를 추적합니다.(NavMesh)
                    //4.공격용 Collider를 비활성화 합니다.
                    ObjectSpawner.objectSpawner.StopSpawning();
                    SetBoolAnimation(TreeState.Run.ToString());
                    StartNavigtaion(stat.RunSpeed);
                    ToggleAttackCollider(false);
                })
                .Condition(() =>
                {
                    //감지거리 내이며, Trigger에 감지되지 않았을 시 true
                    Debug.Log("감지중 입니다.");
                    return IsTargetDetected();
                },
                state =>
                {
                    //갱신 주기마다, 목적지가 초기화 됩니다.
                    Debug.Log("감지거리 내입니다.");
                    if ((state.LastResetDestinationTime + stat.ResetDestinationDelay) <= Time.time)
                    {
                        StartNavigtaion(stat.RunSpeed);
                        state.LastResetDestinationTime = Time.time;
                    }
                })
                .Condition(() =>
                {
                    //감지거리 내를 벗어났을 경우
                    return !IsTargetDetected();
                },
                state =>
                {
                    //Idle state로 전환
                    state.Parent.ChangeState(TreeState.EnrangedIdle.ToString());
                })
                .Condition(() =>
                {
                    //공격 사거리 내로 들어왔을 경우
                    return IsTargetReached();
                },
                state =>
                {
                    //공격 쿨타임이 돌았을 경우에만 Attack state로 전환합니다.
                    if ((state.AttackedTime + stat.AttackDelay) <= Time.time)
                    {
                        state.Parent.ChangeState(TreeState.EnrangedAttack.ToString());
                        state.AttackedTime = Time.time;
                    }
                })
                .End()
            .State<State>(TreeState.EnrangedAttack.ToString())//사정거리 내에 들어오면 일반 공격합니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.EnrangedAttack.ToString()} State");
                    //일반공격 애니메이션 3번이 반드시 일어나도록 구현합니다.
                    //공격용 Collider를 활성화 합니다.
                    SetTriggerAnimation(TreeState.EnrangedAttack.ToString());
                    ToggleAttackCollider(true);
                })
                .End()
            .State<State>(TreeState.Die.ToString())//사정거리 내에 들어오면 일반 공격합니다.
                .Enter(state =>
                {
                    Debug.Log($"Entering {TreeState.Die.ToString()} State");
                    //일반공격 애니메이션 3번이 반드시 일어나도록 구현합니다.
                    //공격용 Collider를 활성화 합니다.
                    Debug.Log("나죽어");
                    SetDie();
                })
                .End()
            .Build();

        rootState.ChangeState(TreeState.Idle.ToString());//초기 상태 Idle
    }

    /// <summary>
    /// 맞았을 때, 이펙트가 발생되지 않습니다.
    /// </summary>
    /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
    public override void SetDamaged(float damage)
    {
        if(IsInvulnerable)//무적인지
        {
            Debug.Log("무적입니다.");
            return;
        }
        Debug.Log("damage입힘");
        stat.CurrentHp -= damage;

        if( stat.CurrentHp / stat.MaxHp <= 0.3f )
        {
            //광폭화될 수 있는 상태값으로 변경합니다.
            if(!isEnranged)
            {
                rootState.ChangeState(TreeState.Enranged.ToString());
            }
        }

        if (stat.CurrentHp <= 0f)
        {
            if (isActive)//한번 죽으면 바뀌는 상태값입니다.
            {
                //사망 시 처리할 로직을 담은 메소드를 집어넣습니다.
                rootState.ChangeState(TreeState.Die.ToString());
            }
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
        if (IsInvulnerable)//무적인지
        {
            Debug.Log("무적입니다.");
            return;
        }
        Debug.Log("damage입힘");
        stat.CurrentHp -= damage;
        if (stat.CurrentHp / stat.MaxHp <= 0.3f)
        {
            //광폭화될 수 있는 상태값으로 변경합니다.
            if (!isEnranged)
            {
                rootState.ChangeState(TreeState.Enranged.ToString());
            }
        }

        if (stat.CurrentHp <= 0f)
        {
            if(isActive)//한번 죽으면 바뀌는 상태값입니다.
            {
                //사망 시 처리할 로직을 담은 메소드를 집어넣습니다.
                rootState.ChangeState(TreeState.Die.ToString());
            }
        }
    }


    /// <summary>
    /// 일반공격 3개가 공유하는 데미지 입히는 메소드 입니다.
    /// </summary>
    public void IsAttacked()
    {
        playerObject.GetComponent<Player_Health>().TakeDamage(stat.PhysicalDamage);
    }

    /// <summary>
    /// 팔에 달린 두 Collider를 활성화/비활성화 하는 메소드 입니다.
    /// Idle State 시, Collider를 비활성화 합니다. 
    /// Attack State 시, Collider를 활성화 합니다.
    /// </summary>
    private void ToggleAttackCollider(bool changeToggle)
    {
        foreach (Collider attackCollider in AttackColliders)
        {
            attackCollider.enabled = changeToggle;
        }
    }
    /// <summary>
    /// Ent가 중앙에 있는지를 감지합니다.
    /// </summary>
    /// <returns>중앙에 근접할 시, true를 반환합니다.</returns>
    private bool IsEntInCenter()
    {
        bool isEntInCenter = false;
        Vector3 navEndPosition = new Vector3(skeletonNav.pathEndPosition.x, 
                                            0, skeletonNav.pathEndPosition.z);
        Vector3 TreeEntSpawnPosition = new Vector3(TreeEntBoxCollider.transform.position.x,
                                    0, TreeEntBoxCollider.transform.position.z);
        Debug.Log(Vector3.Distance(navEndPosition, TreeEntSpawnPosition));
        if(Vector3.Distance(navEndPosition, TreeEntSpawnPosition) < 2f)
        {
            Debug.Log(skeletonNav.remainingDistance);
            if (skeletonNav.remainingDistance <= 1f)
            {
                isEntInCenter = true;
            }
        }
        return isEntInCenter;
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
        if (distanceToPlayer <= stat.DetectionRange)
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
    /// SpawnPoint의 Collider 밖으로 나갔는지를 판단합니다.
    /// </summary>
    /// <returns>나갔으면 true를 반환</returns>
    private bool IsOutsideBounds()
    {
        bool isOutsideBounds = false;
        Vector3 minPosition = TreeEntBoxCollider.bounds.min;//최소 좌표
        Vector3 maxPosition = TreeEntBoxCollider.bounds.max;//최대 좌표
        if (transform.position.x <= minPosition.x || transform.position.x >= maxPosition.x
        || transform.position.z <= minPosition.z || transform.position.z >= maxPosition.z)
        {
            isOutsideBounds = true;
        }
        return isOutsideBounds;
    }

    /// <summary>
    /// 몹이 소환될 때 실행됩니다.
    /// 공격 사거리 * 2 만큼 capsule collider 크기를 조절합니다.
    /// Active 속성 : 살아있는지를 판별합니다.
    /// </summary>
    private void Respawn()
    {
        this.isActive = true;
        this.stat.CurrentHp = this.stat.MaxHp;
        GetComponent<CapsuleCollider>().enabled = true;
        foreach (CapsuleCollider capsule in AttackColliders)
        {
            capsule.height = stat.AttackRange * 2;
        }
        playerMask = playerObject.layer;
    }
    /// <summary>
    /// 사망 시, 가라앉는 코루틴이 실행됩니다.
    /// 강화 포인트도 상승합니다.
    /// </summary>
    public void SetDie()
    {
        this.isActive = false;
        GetComponent<CapsuleCollider>().enabled = false;
        if (!IsAnimationPlaying(TreeState.Die.ToString()))
        {
            SetTriggerAnimation(TreeState.Die.ToString());
            StopNavigtaion();
            StartCoroutine(Sinking());
            //playerObject.GetComponent<Player_Health>().ADDExp();
        }
    }

    /// <summary>
    /// 가라앉기 시작하고 3초 후
    /// ObjectPool로 돌아가기
    /// </summary>
    /// <returns></returns>
    private IEnumerator Sinking()
    {
        yield return new WaitForSeconds(1f);
        float timer = 0f;
        float sinkingStartTime = 3.4f;
        float destroyTime = 5f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > sinkingStartTime)
            {
                transform.position += Vector3.down * Time.fixedDeltaTime * 0.5f;
            }
            if (timer > destroyTime)
            {
                GetComponent<CapsuleCollider>().enabled = false;
                ObjectPool.objectPool.PoolObject(gameObject);
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }



}
