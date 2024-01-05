using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoothit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            // �浹�� Collider�� �����մϴ�.
            Collider hitCollider = hitInfo.collider;

            // ���⿡ ���ϴ� �۾��� �����մϴ�.
            Debug.Log("Hit object: " + hitCollider.gameObject.name);
        }
    }
}
