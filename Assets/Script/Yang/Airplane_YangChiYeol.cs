using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane_YangChiYeol : MonoBehaviour
{
    [Tooltip("비행기 propeller의 Transform을 저장합니다.")]
    [SerializeField] private Transform propellerTransform;
    [Tooltip("비행기 propeller가 초당 돌아가는 각도입니다.")]
    [SerializeField] private float rotationSpeed  = 36f;
    [Tooltip("비행기의 목표 좌표를 가진 배열입니다. Hierachy창에 있는 Object 넣으면 그 위치로 이동합니다.")]
    [SerializeField] private Transform[] TargetPositions;
    [Tooltip("비행기의 초속입니다.")]
    [SerializeField] private float moveSpeed = 5f;
    /// <summary>
    /// 현재 TargetPosition 배열의 index입니다.
    /// </summary>
    private int currentTargetIndex = 0;
    //조건2 : 비행기는 특정 두 지점을 왕복하며 이동한다.(이하 웨이포인트)
    //조건3 : 비행기가 이동할 때는 목표로 하는 웨이포인트를 바라보아야 한다.
    //조건4 : 비행기가 이동할 때는 비행기에 달려있는 프로펠러도 돌아가야한다.
    private Vector3 moveDirection;
    private void Start()
    {
        moveDirection = TargetPositions[currentTargetIndex].position - transform.position;
        moveDirection.Normalize();
        transform.rotation = Quaternion.LookRotation(moveDirection);
        //제 환경에서 Forward위치를 바라보지 않아 보정값 추가하였습니다.제대로 안돌아가면 아래만 좀 조정해 주세요
        transform.rotation *= Quaternion.Euler(new Vector3(0, 90, 0));
    }
    private void Update()
    {
        Debug.Log(transform.forward);
        //프로펠러 돌아가는 로직
        float rotationAmount = rotationSpeed * Time.deltaTime;
        Quaternion currentRotation = propellerTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(36f * Time.deltaTime, 0, 0));
        propellerTransform.rotation *= targetRotation;

        //이동하는 로직
        if(Vector3.Distance(transform.position, TargetPositions[currentTargetIndex].position) >= 0.1f)
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
        else
        {
            if(currentTargetIndex < TargetPositions.Length - 1)
            {
                currentTargetIndex++;
            }
            else
            {
                currentTargetIndex = 0;
            }
            moveDirection = TargetPositions[currentTargetIndex].position - transform.position;
            moveDirection.Normalize();
            transform.rotation = Quaternion.LookRotation(moveDirection);
            //제 환경에서 Forward위치를 바라보지 않아 보정값 추가하였습니다.제대로 안돌아가면 아래만 좀 조정해 주세요
            transform.rotation *= Quaternion.Euler(new Vector3(0, 90, 0));
        }

    }





}
