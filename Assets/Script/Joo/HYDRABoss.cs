using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYDRABoss : MonoBehaviour
{
    HYDRA hydra;
    HYDRAStat stat;
    [SerializeField] Transform player;
    float rotationSpeed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        stat = GetComponent<HYDRAStat>();

    }

    float normalTimer = 3f;
    float skillTimer = 5f;
    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if(distanceToPlayer <= stat.SkillattackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveToPoint();
            }
        }

    }

    void MoveTowardsPlayer()
    {
        Vector3 targetPosition = player.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, stat.WalkSpeed * Time.deltaTime);

        Vector3 direction = (targetPosition-transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    [SerializeField] Transform[] points;
    int currentpointIndex = 0;

    void MoveToPoint()
    {
        if(currentpointIndex < points.Length)
        {
            Vector3 targetPosition = points[currentpointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, stat.WalkSpeed * Time.deltaTime);
            if(transform.position == targetPosition)
            {
                currentpointIndex++;
                if(currentpointIndex >= points.Length)
                {
                    currentpointIndex = 0;
                }
            }

            Vector3 direction = (targetPosition-transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }


}


// �÷��̾ ���� �� �⺻���� �̵�
// + �÷��̾ �ٶ󺸸� or ���� ������ �ٶ󺸸� �̵�



// ������ �������� �Ϲݰ��� ..?   // HeavyHit, Grab, Fivehit, Threehit
// ������ �������� ��ų���� ..?   // Roar
// ��ų ���ݽ� �ٴڿ� �����ϴ� ���� ǥ�� ( ���� �� ������ �ð����� ǥ�� �� ���ݽ��۰� ���ÿ� ���־� �� )
// �÷��̾��� ���ݰ� ����ü���� ��ȣ�ۿ�
// ������ ���ݰ� �÷��̾�ü���� ��ȣ�ۿ�
// ���� ����  �� Roar Ȱ��ȭ �� ��ƼŬ �߰�
// ������ ������ ������Ʈ�� �浹�� ��ƼŬ ����
