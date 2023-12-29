using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public bool changepos = false;
    [Range(-1f, 2f)]
    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 10.0f;           // 카메라의 y좌표
    public float offsetZ = -10.0f;
    public float turnspeed = 80f;
    public float camspeed = 10f;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    Vector3 TargetPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("aaa");
        float r = Input.GetAxis("Mouse X");
        if (target != null)
        {
            // 대상의 현재 위치에 오프셋을 더하고, 부드러운 이동을 위해 Lerp 사용
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // 대상을 바라보도록 카메라의 회전 조절
            transform.LookAt(target);
        }
        /*TargetPos = new Vector3(
             Target.transform.position.x + offsetX,
             Target.transform.position.y + offsetY,
             Target.transform.position.z + offsetZ
             );

       
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * camspeed);
        //camTr.position = taargetTr.position + (-taargetTr.forward * distance) + (Vector3.up * height);*/
        transform.Rotate(Vector3.up * turnspeed * Time.deltaTime * r);

        if (changepos)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                changepos = false;
                offsetY = 2f;
                offsetZ = 2F;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                changepos = true;
                offsetZ = -0.5f;
                offsetY = 1.6f;
            }
        }
    }
}
