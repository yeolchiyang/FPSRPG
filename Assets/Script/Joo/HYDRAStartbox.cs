using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HYDRAStartbox : MonoBehaviour
{
    [SerializeField] Transform SMap;
    [SerializeField] Transform Player;
    Collider myColl;

    public float fallSpeed = 30f;

    private void Start()
    {
        myColl = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartFalling();
        }
    }

    void StartFalling()
    {
        InvokeRepeating("Fall", 0f, 0.02f);
    }

    void Fall()
    {
        if(SMap.transform.position.y > -19.68)
        {
            SMap.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            if (SMap.transform.position.y < -15)
            {
                myColl.enabled = false;
            }
        }
        else
        {
            Player.SetParent(null);
        }

    }

}
