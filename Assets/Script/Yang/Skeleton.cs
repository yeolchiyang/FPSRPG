using RSG;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

namespace Yang{
    public abstract class Skeleton : MonoBehaviour
    {
        public bool isActive = true;
        protected EnemyStat stat;
        public IState rootState;//���¸� �����ϴ� Ŭ�����Դϴ�.
        protected UnityEngine.AI.NavMeshAgent skeletonNav;
        [SerializeField] protected Animator skeletonAnimator;
        [SerializeField] protected GameObject skeletonHitEffect;

        //Player�� singleton���� �����ϰ� �ִ� ��ü�� ���� ��� ��ü�� ��
        protected GameObject playerObject;

        private void Awake()
        {
            skeletonNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            stat = GetComponent<EnemyStat>();
            playerObject = GameObject.FindWithTag("Player");
        }

        /// <summary>
        /// ��� Enemy�� ������ �־�� �ϴ� �޼ҵ� �Դϴ�.
        /// �¾��� ��, ����Ʈ�� �߻����� �ʽ��ϴ�.
        /// </summary>
        /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
        public virtual void SetDamaged(float damage)
        {
            
        }
        /// <summary>
        /// ��� Enemy�� ������ �־�� �ϴ� �޼ҵ� �Դϴ�.
        /// Raycast�� �̿��� ���� ��, ��ȣ�ۿ� ������ �޼ҵ� �Դϴ�.
        /// ���� ��ҿ� ����Ʈ�� ����ϴ�.
        /// </summary>
        /// <param name="hit">RaycastHit Object �־��ּ���</param>
        /// <param name="damage">���ϰ��� �ϴ� ������ �־��ּ���</param>
        public virtual void SetDamaged(RaycastHit hit, float damage)
        {
            
        }
        /// <summary>
        /// NavMesh�� �����մϴ�. ��� ������Ʈ�� ���� �����մϴ�.
        /// </summary>
        protected void StopNavigtaion()
        {
            if (!skeletonNav.isStopped)
            {
                skeletonNav.isStopped = true;
                skeletonNav.updatePosition = false;
                skeletonNav.updateRotation = false;
                skeletonNav.velocity = Vector3.zero;
            }
        }
        /// <summary>
        /// NavMesh�� ���� �Ǵ� ������մϴ�. ��� ������Ʈ�� ���� �����մϴ�.
        /// </summary>
        /// <param name="speed">1�ʸ��� speed��ŭ�� �Ÿ��� �̵��մϴ�.</param>
        protected void StartNavigtaion(float speed)
        {
            skeletonNav.isStopped = false;
            skeletonNav.ResetPath();//ResetPath -> SetDestination �ؾ� ���۵� �մϴ�.
            skeletonNav.SetDestination(playerObject.transform.position);
            skeletonNav.updatePosition = true;
            skeletonNav.updateRotation = true;
            skeletonNav.speed = speed;
        }

    }
}

