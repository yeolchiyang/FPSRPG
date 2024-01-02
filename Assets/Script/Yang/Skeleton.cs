using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yang{
    public class Skeleton : MonoBehaviour
    {
        private EnemyStat stat;


        private UnityEngine.AI.NavMeshAgent skeletonNav;
        //임시, 테스트 후 삭제
        [SerializeField] private Transform targetPoint;

        private void Awake()
        {
            skeletonNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            stat = GetComponent<EnemyStat>();
        }

        private void Start()
        {
            skeletonNav.SetDestination(targetPoint.position);
        }
        private void Update()
        {
            //moveTo();
        }

        //임시
        private void moveTo()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 moveDirection = new Vector3(x, 0f, z);
            moveDirection.Normalize();

            transform.Translate(moveDirection * Time.deltaTime * 5f);
        }
    }
}

