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
    [SerializeField] private float idleSpeed;
    [SerializeField] private float runSpeed;

    public float Hp { get { return hp; } set { hp = value; } }
    public float PhysicalDamage { get { return physicalDamage; } set { physicalDamage = value; } }
    public float IdleSpeed { get { return idleSpeed; } set { idleSpeed = value; } }
    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }

}
