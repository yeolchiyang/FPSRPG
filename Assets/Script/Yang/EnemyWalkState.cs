using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : AbstractState
{
    /// <summary>
    /// �������� ���� �� �ð��� ����մϴ�.
    /// </summary>
    public float LastResetDestinationTime { get; set; } = 0f;
}
