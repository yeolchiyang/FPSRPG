using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stop : MonoBehaviour
{
    npc npc;
    public GameObject stopGameObject;
    // Start is called before the first frame update
    void Start()
    {
        npc = stopGameObject.GetComponent<npc>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            npc.PlayerStop();
        }
    }
}
