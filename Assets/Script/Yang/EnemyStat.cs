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
    [Tooltip("�ִ� ü��")]
    [SerializeField] private float maxHp;
    [Tooltip("���� ü��")]
    [SerializeField] private float currentHp;
    [Tooltip("���� ������")]
    [SerializeField] private float physicalDamage;
    [Tooltip("�ȴ� �ӵ�")]
    [SerializeField] private float walkSpeed;
    [Tooltip("�޷����� �ӵ�(�޸��� ���� ���� �� ����)")]
    [SerializeField] private float runSpeed;
    [Tooltip("���� ��Ÿ�(����Ʈ �� �̻�)")]
    [SerializeField] private float detectionRange;
    [Tooltip("�⺻ ���� ��Ÿ�")]
    [SerializeField] private float attackRange;
    [Tooltip("�⺻ ���� �ֱ�")]
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
