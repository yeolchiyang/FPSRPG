using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    [SerializeField] Transform player;
    public Camera mainCamera;
    Vector2 miniMapSize = new Vector2(473.9351f, 277.4289f);
    Vector3 mapCenter = new Vector3(-11.42527f, 2.413804f, 8.816338f);

    void Update()
    {
        if (player != null && mainCamera != null)
        {
            Vector3 playerRelativePos = player.position - mapCenter;

            Vector3 playerViewportPos = mainCamera.WorldToViewportPoint(playerRelativePos);

            float miniMapX = playerViewportPos.x * miniMapSize.x - miniMapSize.x / 2f;
            float miniMapZ = playerViewportPos.y * miniMapSize.y - miniMapSize.y / 2f;


            Vector3 miniMapPosition = new Vector3(
               miniMapX,
                transform.position.y,
                miniMapZ
            );

            transform.position = miniMapPosition;
        }
    }
}
