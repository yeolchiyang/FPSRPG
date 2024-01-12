using RSG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeEntWalkState : AbstractState
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

}
