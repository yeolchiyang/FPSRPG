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

    [SerializeField] private float hp;
    [SerializeField] private float physicalDamage;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    public float Hp { get { return hp; } set { hp = value; } }
    public float PhysicalDamage { get { return physicalDamage; } set { physicalDamage = value; } }
    public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
    public float AttackRange { get { return attackRange; } set { attackRange = value; } }
    public float AttackDelay { get { return attackDelay; } set { attackDelay = value; } }
}
