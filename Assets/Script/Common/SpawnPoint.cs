using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 스폰장소를 표시하기만 하는 스크립트 입니다. 게임화면에는 나오지 않습니다.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    public enum colors
    {
        Red,
        Blue,
        Green
    }

    [SerializeField] private colors currentColor;

    public void OnDrawGizmos()
    {
        //colors colors = colors;
        switch(this.currentColor)
        {
            case colors.Red:
                Handles.color = Color.red;
                break;
            case colors.Green:
                Handles.color = Color.green;
                break;
            case colors.Blue:
                Handles.color = Color.blue;
                break;
        }

        Handles.CircleHandleCap(
            0,
            transform.position + new Vector3(0f, 3f, 0f),
            transform.rotation * Quaternion.LookRotation(Vector3.up),
            5f,
            EventType.Repaint
        );
    }
}
