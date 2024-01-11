using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Stat
//{
//    hp,
//    physicalDamage,
//    idleSpeed,
//    runSpeed
//}

public class EnemyStat : MonoBehaviour
{
    [Tooltip("최대 체력")]
    [SerializeField] private float maxHp;
    [Tooltip("현재 체력")]
    [SerializeField] private float currentHp;
    [Tooltip("물리 데미지")]
    [SerializeField] private float physicalDamage;
    [Tooltip("걷는 속도")]
    [SerializeField] private float walkSpeed;
    [Tooltip("달려가는 속도(달리는 상태 보유 몹 한정)")]
    [SerializeField] private float runSpeed;
    [Tooltip("감지 사거리(엘리트 몹 이상)")]
    [SerializeField] private float detectionRange;
    [Tooltip("기본 공격 사거리")]
    [SerializeField] private float attackRange;
    [Tooltip("기본 공격 주기")]
    [SerializeField] private float attackDelay;
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    public float CurrentHp { get { return currentHp; } set { currentHp = value; } }
    public float PhysicalDamage { get { return physicalDamage; } set { physicalDamage = value; } }
    public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
    public float DetectionRange { get { return detectionRange; } set { detectionRange = value; } }
    public float AttackRange { get { return attackRange; } set { attackRange = value; } }
    public float AttackDelay { get { return attackDelay; } set { attackDelay = value; } }
}
