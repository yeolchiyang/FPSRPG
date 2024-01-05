using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 스폰장소를 표시하기만 하는 스크립트 입니다. 게임화면에는 나오지 않습니다.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        Handles.color = Handles.yAxisColor;
        Handles.CircleHandleCap(
            0,
            transform.position + new Vector3(0f, 3f, 0f),
            transform.rotation * Quaternion.LookRotation(Vector3.up),
            5f,
            EventType.Repaint
        );
    }
}
