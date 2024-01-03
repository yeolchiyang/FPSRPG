using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : AbstractState
{
    private float attackCount;
    public float AttackCount {  get { return attackCount; } set { attackCount = value; } }   
}
