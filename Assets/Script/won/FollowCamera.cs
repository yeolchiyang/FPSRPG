using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Player_animation;

public class FollowCamera : MonoBehaviour
{
    
    public Transform targetTr;
    Transform camTr;
    public bool changepos = false;
    [Range(-1f, 2f)]
    public float distance = 2.0f;            // 카메라의 x좌표
    public float height = 2.0f;           // 카메라의 y좌표
    public float turnspeed = 80f;
    public float camspeed = 10f;
    private float rotationY;
    float a;
    public float b =0;
    public float rot_X=8;

    // Start is called before the first frame update
    void Start()
    {
        camTr = GetComponent<Transform>();
        rot_X = 18;
        changepos = true;
        distance = 0f;
        height = 1.9f;
        b = 0;

    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
        float r = Input.GetAxis("Mouse X");
        a += turnspeed * Time.deltaTime * r;
        b = b+Time.deltaTime;
        // transform.Rotate(0,turnspeed * Time.deltaTime * r,0,Space.World);

        if (changepos)
        {
            transform.rotation = Quaternion.Euler(rot_X, a, 0);
            camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
            if (Input.GetKeyDown(KeyCode.Q)&& b>=0.7f)
            {
                rot_X = 8;
                changepos = false;
                distance = 2f;
                height = 2F;
                b= 0;
            }
        }
        else if (!changepos)
        {
            transform.rotation = Quaternion.Euler(rot_X, a, 0);
            camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
            if (Input.GetKeyDown(KeyCode.Q)&&b>=0.7)
            {
                rot_X = 18;
                changepos = true;
                distance = 0f;
                height = 1.9f;
                b= 0;
            }
        }
    }
}
