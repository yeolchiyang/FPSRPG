using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYDRAStat : MonoBehaviour
{
    [Tooltip("걷는 속도")]
    [SerializeField] private float walkSpeed;  // 1.25
    [Tooltip("최대 체력")]
    [SerializeField] private float maxHp;  // 1300
    [Tooltip("현재 체력")]
    [SerializeField] private float currentHp;
    [Tooltip("기본 데미지")]
    [SerializeField] private float normalDamage;  // 8
    [Tooltip("스킬 데미지")]
    [SerializeField] private float skillDamage;  // 13
    [Tooltip("기본 공격 사거리")]
    [SerializeField] private float normalattackRange;  // 25
    [Tooltip("스킬 공격 사거리 && Player 인식 최대 범위")]
    [SerializeField] private float skillattackRange;  // 50
    [Tooltip("공격 주기")]
    [SerializeField] private float attackDelay;  // 3
    public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    public float CurrentHp { get { return currentHp; } set { currentHp = value; } }
    public float NormalDamage { get { return normalDamage; } set { normalDamage = value; } }
    public float SkillDamage { get { return skillDamage; } set { skillDamage = value; } }
    public float NormalAttackRange { get { return normalattackRange; } set { normalattackRange = value; } }
    public float SkillattackRange { get { return skillattackRange; } set { skillattackRange = value; } }
    public float AttackDelay { get { return attackDelay; } set { attackDelay = value; } }
}
