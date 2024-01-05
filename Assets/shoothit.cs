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
            // 충돌한 Collider에 접근합니다.
            Collider hitCollider = hitInfo.collider;

            // 여기에 원하는 작업을 수행합니다.
            Debug.Log("Hit object: " + hitCollider.gameObject.name);
        }
    }
}
