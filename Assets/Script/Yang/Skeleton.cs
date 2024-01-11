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
        public IState rootState;//상태를 관리하는 클래스입니다.
        protected UnityEngine.AI.NavMeshAgent skeletonNav;
        [SerializeField] protected Animator skeletonAnimator;
        [SerializeField] protected GameObject skeletonHitEffect;

        //Player를 singleton으로 저장하고 있는 객체가 있을 경우 대체할 것
        protected GameObject playerObject;

        private void Awake()
        {
            skeletonNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            stat = GetComponent<EnemyStat>();
            playerObject = GameObject.FindWithTag("Player");
        }

        /// <summary>
        /// 모든 Enemy가 가지고 있어야 하는 메소드 입니다.
        /// 맞았을 때, 이펙트가 발생되지 않습니다.
        /// </summary>
        /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
        public virtual void SetDamaged(float damage)
        {
            
        }
        /// <summary>
        /// 모든 Enemy가 가지고 있어야 하는 메소드 입니다.
        /// Raycast를 이용한 공격 시, 상호작용 가능한 메소드 입니다.
        /// 맞은 장소에 이펙트가 생깁니다.
        /// </summary>
        /// <param name="hit">RaycastHit Object 넣어주세요</param>
        /// <param name="damage">가하고자 하는 데미지 넣어주세요</param>
        public virtual void SetDamaged(RaycastHit hit, float damage)
        {
            
        }
        /// <summary>
        /// NavMesh를 중지합니다. 모든 오브젝트에 적용 가능합니다.
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
        /// NavMesh를 시작 또는 재시작합니다. 모든 오브젝트에 적용 가능합니다.
        /// </summary>
        /// <param name="speed">1초마다 speed만큼의 거리를 이동합니다.</param>
        protected void StartNavigtaion(float speed)
        {
            skeletonNav.isStopped = false;
            skeletonNav.ResetPath();//ResetPath -> SetDestination 해야 재작동 합니다.
            skeletonNav.SetDestination(playerObject.transform.position);
            skeletonNav.updatePosition = true;
            skeletonNav.updateRotation = true;
            skeletonNav.speed = speed;
        }

    }
}

