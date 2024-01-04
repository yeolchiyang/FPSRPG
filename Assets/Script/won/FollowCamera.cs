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
    float First_MaxY = 45f;
    float third_MaxY = 30f;
    [Range(-1f, 2f)]
    public float distance = 2.0f;            // 카메라의 x좌표
    public float height = 2.0f;           // 카메라의 y좌표
    float turnspeed = 60f;
    public float camspeed = 10f;
    float ViewX;
    float FirstView =0;
    float thirdView = 0;
    public float rot_X=8;
    float ViewChanage = 0;

    // Start is called before the first frame update
    void Start()
    {
        camTr = GetComponent<Transform>();
        rot_X = 18;
        changepos = true;
        distance = 0f;
        height = 1.9f;
        ViewChanage = 0;

    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
        float rotationX = Input.GetAxis("Mouse X");
        float rotationY = Input.GetAxis("Mouse Y");
        FirstView += turnspeed * Time.deltaTime * rotationY;
        FirstView = Mathf.Clamp(FirstView, -10, First_MaxY);
        thirdView += turnspeed * Time.deltaTime * rotationY;
        thirdView = Mathf.Clamp(thirdView, -10, third_MaxY);
        ViewX += turnspeed * Time.deltaTime * rotationX;
        ViewChanage = ViewChanage + Time.deltaTime;
        // transform.Rotate(0,turnspeed * Time.deltaTime * r,0,Space.World);

        if (changepos)
        {
            thirdView = 0;
            transform.rotation = Quaternion.Euler(rot_X- FirstView, ViewX, 0);
            camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
            if (Input.GetKeyDown(KeyCode.Q)&& ViewChanage >= 0.7f)
            {
                rot_X = 8;
                changepos = false;
                distance = 2f;
                height = 2F;
                ViewChanage = 0;
            }
        }
        else if (!changepos)
        {
            FirstView = 0;
            transform.rotation = Quaternion.Euler(rot_X - thirdView, ViewX, 0);
            camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
            if (Input.GetKeyDown(KeyCode.Q)&& ViewChanage >= 0.7)
            {
                rot_X = 18;
                changepos = true;
                distance = 0f;
                height = 1.9f;
                ViewChanage = 0;
            }
        }
    }
}
