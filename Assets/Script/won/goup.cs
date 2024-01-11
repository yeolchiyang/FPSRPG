using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goup : MonoBehaviour
{
    GameObject startup;
    public GameObject backpos;
    // Start is called before the first frame update
    void Start()
    {
        startup = GameObject.FindWithTag("startup");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.position = startup.transform.position;
            Invoke("OnDestroy", 1f);
        }
    }
    public void OnDestroy()
    {
        backpos.transform.position = transform.position;
        Destroy(gameObject);
    }
}
