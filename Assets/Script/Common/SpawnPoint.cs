using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ������Ҹ� ǥ���ϱ⸸ �ϴ� ��ũ��Ʈ �Դϴ�. ����ȭ�鿡�� ������ �ʽ��ϴ�.
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
