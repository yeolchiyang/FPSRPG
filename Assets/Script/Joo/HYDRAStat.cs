using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYDRAStat : MonoBehaviour
{
    [Tooltip("�ȴ� �ӵ�")]
    [SerializeField] private float walkSpeed;  // 1.25
    [Tooltip("�ִ� ü��")]
    [SerializeField] private float maxHp;  // 1300
    [Tooltip("���� ü��")]
    [SerializeField] private float currentHp;
    [Tooltip("�⺻ ������")]
    [SerializeField] private float normalDamage;  // 8
    [Tooltip("��ų ������")]
    [SerializeField] private float skillDamage;  // 13
    [Tooltip("�⺻ ���� ��Ÿ�")]
    [SerializeField] private float normalattackRange;  // 40
    [Tooltip("��ų ���� ��Ÿ� && Player �ν� �ִ� ����")]
    [SerializeField] private float skillattackRange;  // 70
    [Tooltip("�⺻ ���� �ֱ�")]
    [SerializeField] private float normalattackDelay;  // 3
    [Tooltip("��ų ���� �ֱ�")]
    [SerializeField] private float skillattackDelay;  // 5
    public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    public float CurrentHp { get { return currentHp; } set { currentHp = value; } }
    public float NormalDamage { get { return normalDamage; } set { normalDamage = value; } }
    public float SkillDamage { get { return skillDamage; } set { skillDamage = value; } }
    public float NormalAttackRange { get { return normalattackRange; } set { normalattackRange = value; } }
    public float SkillattackRange { get { return skillattackRange; } set { skillattackRange = value; } }
    public float NormalattackDelay { get { return normalattackDelay; } set { normalattackDelay = value; } }
    public float SkillattackDelay { get { return skillattackDelay; } set { skillattackDelay = value; } }
}
