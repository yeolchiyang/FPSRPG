using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntHitDetetion : MonoBehaviour
{

    private LayerMask playerMask;

    private void OnEnable()
    {
        playerMask = GetComponentInParent<TreeEntController>().playerMask;
    }

    private void OnTriggerEnter(Collider other)
    {
        int collidedLayer = other.gameObject.layer;
        int playerMaskValue = 1 << playerMask.value;
        if ( ((1 << collidedLayer) & playerMaskValue) != 0)
        {
            Debug.Log("Player is hitted!");
        }

    }
}
