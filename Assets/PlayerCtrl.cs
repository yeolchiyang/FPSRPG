using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Transform tr;
    public float speed = 10f;
    public float turnspeed = 80f;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis ("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward*v) + (Vector3.right*h);

        tr.Translate(moveDir.normalized * speed * Time.deltaTime);
        tr.Rotate(Vector3.up * turnspeed * Time.deltaTime * r);
    }
}
