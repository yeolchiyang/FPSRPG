using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane_YangChiYeol : MonoBehaviour
{
    [Tooltip("����� propeller�� Transform�� �����մϴ�.")]
    [SerializeField] private Transform propellerTransform;
    [Tooltip("����� propeller�� �ʴ� ���ư��� �����Դϴ�.")]
    [SerializeField] private float rotationSpeed  = 36f;
    [Tooltip("������� ��ǥ ��ǥ�� ���� �迭�Դϴ�. Hierachyâ�� �ִ� Object ������ �� ��ġ�� �̵��մϴ�.")]
    [SerializeField] private Transform[] TargetPositions;
    [Tooltip("������� �ʼ��Դϴ�.")]
    [SerializeField] private float moveSpeed = 5f;
    /// <summary>
    /// ���� TargetPosition �迭�� index�Դϴ�.
    /// </summary>
    private int currentTargetIndex = 0;
    //����2 : ������ Ư�� �� ������ �պ��ϸ� �̵��Ѵ�.(���� ��������Ʈ)
    //����3 : ����Ⱑ �̵��� ���� ��ǥ�� �ϴ� ��������Ʈ�� �ٶ󺸾ƾ� �Ѵ�.
    //����4 : ����Ⱑ �̵��� ���� ����⿡ �޷��ִ� �����緯�� ���ư����Ѵ�.
    private Vector3 moveDirection;
    private void Start()
    {
        moveDirection = TargetPositions[currentTargetIndex].position - transform.position;
        moveDirection.Normalize();
        transform.rotation = Quaternion.LookRotation(moveDirection);
        //�� ȯ�濡�� Forward��ġ�� �ٶ��� �ʾ� ������ �߰��Ͽ����ϴ�.����� �ȵ��ư��� �Ʒ��� �� ������ �ּ���
        transform.rotation *= Quaternion.Euler(new Vector3(0, 90, 0));
    }
    private void Update()
    {
        Debug.Log(transform.forward);
        //�����緯 ���ư��� ����
        float rotationAmount = rotationSpeed * Time.deltaTime;
        Quaternion currentRotation = propellerTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(36f * Time.deltaTime, 0, 0));
        propellerTransform.rotation *= targetRotation;

        //�̵��ϴ� ����
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
            //�� ȯ�濡�� Forward��ġ�� �ٶ��� �ʾ� ������ �߰��Ͽ����ϴ�.����� �ȵ��ư��� �Ʒ��� �� ������ �ּ���
            transform.rotation *= Quaternion.Euler(new Vector3(0, 90, 0));
        }

    }





}
