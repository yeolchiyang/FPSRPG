using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class back : MonoBehaviour
{
    GameObject backpos;

    void Start()
    {
        
    }
    private void Update()
    {
        backpos = GameObject.Find("backpos");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Player 오브젝트를 저장된 GetPosition의 좌표로 이동
            other.transform.position = backpos.transform.position;
            Destroy(gameObject);
            Destroy(backpos);
        }
    }

}
