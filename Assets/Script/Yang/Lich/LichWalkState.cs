using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichWalkState : AbstractState
{
    /// <summary>
    /// 일반 공격을 언제 하였는지 기록합니다.
    /// </summary>
    public float AttackedTime { get; set; } = 0f;
    /// <summary>
    /// Stomp 공격을 언제 하였는지 기록합니다.
    /// </summary>
    public float StompedTime { get; set; } = 0f;
    /// <summary>
    /// JumpSmash 공격을 언제 하였는지 기록합니다.
    /// </summary>
    public float JumpSmashedTime { get; set; } = 0f;
    /// <summary>
    /// 목적지가 갱신 된 시간을 기록합니다.
    /// </summary>
    public float LastResetDestinationTime { get; set; } = 0f;


}
