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
            // Player ������Ʈ�� ����� GetPosition�� ��ǥ�� �̵�
            other.transform.position = backpos.transform.position;
            Destroy(gameObject);
            Destroy(backpos);
        }
    }

}
