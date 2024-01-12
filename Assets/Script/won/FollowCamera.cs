using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Player_animation;
using UnityEngine.UIElements;

public class FollowCamera : MonoBehaviour
{
    public Transform targetTr;
    Transform camTr;
    public bool changepos = false;
    float First_MaxY = 45f;
    float third_MaxY = 30f;
    [Range(-1f, 2f)]
    public float distance = 2.0f;
    public float height = 2.0f;
    float turnspeed = 60f;
    public float camspeed = 10f;
    float ViewX;
    float FirstView = 0;
    float thirdView = 0;
    public float rot_X = 8;
    float ViewChanage = 0;

    void Start()
    {
        camTr = GetComponent<Transform>();
        rot_X = 18;
        changepos = true;
        distance = 0f;
        height = 1.9f;
        ViewChanage = 0;
    }

    void FixedUpdate()
    {
        camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
        float rotationY = Input.GetAxis("Mouse Y");
        FirstView += turnspeed * Time.deltaTime * rotationY;
        FirstView = Mathf.Clamp(FirstView, -10, First_MaxY);
        thirdView += turnspeed * Time.deltaTime * rotationY;
        thirdView = Mathf.Clamp(thirdView, -10, third_MaxY);

        // targetTr의 rotation에서 y 성분을 가져옴
        ViewX = targetTr.rotation.eulerAngles.y;

        ViewChanage += Time.deltaTime;

        if (changepos)
        {
            thirdView = 0;
            transform.rotation = Quaternion.Euler(rot_X - FirstView, ViewX, 0);
            camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
            if (Input.GetKeyDown(KeyCode.Q) && ViewChanage >= 0.7f)
            {
                rot_X = 8;
                changepos = false;
                distance = 2f;
                height = 2f;
                ViewChanage = 0;
            }
        }
        else
        {
            FirstView = 0;
            transform.rotation = Quaternion.Euler(rot_X - thirdView, ViewX, 0);
            camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
            if (Input.GetKeyDown(KeyCode.Q) && ViewChanage >= 0.7)
            {
                rot_X = 18;
                changepos = true;
                distance = 0f;
                height = 1.9f;
                ViewChanage = 0;
            }
        } 
    }
        public void camin()
        {
            FirstView = 0;
            thirdView = 0;
            ViewX = 0;
            transform.forward = targetTr.forward;
        }
        public void camout()
        {
            FirstView = 0;
            thirdView = 0;
            ViewX = 0;
        if (changepos)
        {
            rot_X = 18;
            distance = 0f;
            height = 1.9f;
            ViewChanage = 0;
        }
        else
        {
            rot_X = 8;
            distance = 2f;
            height = 2f;
            ViewChanage = 0;
        }
            transform.rotation = Quaternion.Euler(0, targetTr.rotation.y, 0);

        }


    } 

