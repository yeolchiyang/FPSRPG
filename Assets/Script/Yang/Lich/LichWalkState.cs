using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichWalkState : AbstractState
{
    /// <summary>
    /// �Ϲ� ������ ���� �Ͽ����� ����մϴ�.
    /// </summary>
    public float AttackedTime { get; set; } = 0f;
    /// <summary>
    /// Stomp ������ ���� �Ͽ����� ����մϴ�.
    /// </summary>
    public float StompedTime { get; set; } = 0f;
    /// <summary>
    /// JumpSmash ������ ���� �Ͽ����� ����մϴ�.
    /// </summary>
    public float JumpSmashedTime { get; set; } = 0f;
    /// <summary>
    /// �������� ���� �� �ð��� ����մϴ�.
    /// </summary>
    public float LastResetDestinationTime { get; set; } = 0f;


}
