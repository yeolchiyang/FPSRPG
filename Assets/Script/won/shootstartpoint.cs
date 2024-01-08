using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class shootstartpoint : MonoBehaviour
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
    float FirstView = 0;
    float thirdView = 0;
    public float rot_X = 8;
    float ViewChanage = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion a = transform.rotation;
        float rotationX = Input.GetAxis("Mouse X");
        float rotationY = Input.GetAxis("Mouse Y");
        FirstView += turnspeed * Time.deltaTime * rotationY;
        FirstView = Mathf.Clamp(FirstView, -10, First_MaxY);
        thirdView += turnspeed * Time.deltaTime * rotationY;
        thirdView = Mathf.Clamp(thirdView, -10, third_MaxY);
        ViewX += turnspeed * Time.deltaTime * rotationX;
        ViewChanage = ViewChanage + Time.deltaTime;
        if (changepos)
        {
            thirdView = 0;
            transform.rotation = Quaternion.Euler(-FirstView, ViewX, 0);
            if (Input.GetKeyDown(KeyCode.Q) && ViewChanage >= 0.7f)
            {
                changepos = false;
                ViewChanage = 0;
                transform.rotation = a;
            }
        }
        else if (!changepos)
        {
            FirstView = 0;
            transform.rotation = Quaternion.Euler(-thirdView, ViewX, 0);
            if (Input.GetKeyDown(KeyCode.Q) && ViewChanage >= 0.7)
            {
                transform.rotation = a;
                changepos = true;
                ViewChanage = 0;
            }
        }
    }
}
