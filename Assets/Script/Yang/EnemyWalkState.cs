using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : AbstractState
{
    /// <summary>
    /// 목적지가 갱신 된 시간을 기록합니다.
    /// </summary>
    public float LastResetDestinationTime { get; set; } = 0f;
}
