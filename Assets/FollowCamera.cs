using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowCamera : MonoBehaviour
{
    public Transform targetTr;
    Transform camTr;
    public bool changepos = false;
    [Range(-1f, 2f)]
    public float distance = 0.0f;            // 카메라의 x좌표
    public float height = 2.0f;           // 카메라의 y좌표
    public float turnspeed = 80f;
    public float camspeed = 10f;
    private float rotationY;
    float a;
    public float rot_X=8;

    // Start is called before the first frame update
    void Start()
    {
        camTr = GetComponent<Transform>();
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        float r = Input.GetAxis("Mouse X");
        a += turnspeed * Time.deltaTime * r;
        // transform.Rotate(0,turnspeed * Time.deltaTime * r,0,Space.World);
       

        if (changepos)
        {
            transform.rotation = Quaternion.Euler(0, a, 0);
            camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                changepos = false;
                distance = 2f;
                height = 2F;
                
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(rot_X, a, 0);
            camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                changepos = true;
                distance = -0.5f;
                height = 1.6f;
            }
        }
    }
}
