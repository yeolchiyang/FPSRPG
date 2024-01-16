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
        [SerializeField] protected GameObject HitEffect;

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
        /// NavMesh를 시작 또는 재시작합니다. 
        /// 목적지는 Player 위치 입니다.
        /// 모든 오브젝트에 적용 가능합니다.
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

        /// <summary>
        /// NavMesh를 시작 또는 재시작합니다. 
        /// 목적지를 새로 설정 가능합니다. 
        /// 모든 오브젝트에 적용 가능합니다.
        /// </summary>
        /// <param name="speed">1초마다 speed만큼의 거리를 이동합니다.</param>
        /// <param name="targetPosition"></param>
        protected void StartNavigation(float speed, Vector3 targetPosition)
        {
            skeletonNav.isStopped = false;
            skeletonNav.ResetPath();//ResetPath -> SetDestination 해야 재작동 합니다.
            skeletonNav.SetDestination(targetPosition);
            skeletonNav.updatePosition = true;
            skeletonNav.updateRotation = true;
            skeletonNav.speed = speed;
        }
        protected void SetTriggerAnimation(string state)
        {
            skeletonAnimator.SetTrigger(state);
        }

        protected void SetBoolAnimation(string state)
        {
            // 현재 애니메이터의 모든 파라미터를 가져옵니다.
            AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;

            // 각 파라미터에 대해 반복합니다.
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                // 제외할 파라미터인지 확인하고, 제외되지 않은 경우 값을 false로 설정합니다.
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    if (parameter.name == state)
                    {
                        skeletonAnimator.SetBool(parameter.name, true);
                    }
                    else
                    {
                        skeletonAnimator.SetBool(parameter.name, false);
                    }
                }
            }
        }
        /// <summary>
        /// 공격, 데미지를 입었을 때 빠르게 기존 state로 전환하기 위함입니다.
        /// </summary>
        /// <returns>정의한 상태값의 string 값을 반환합니다.</returns>
        protected string GetBoolAnimationName()
        {
            string parameterName = "";
            // 현재 애니메이터의 모든 파라미터를 가져옵니다.
            AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    //true인 파라미터면
                    if (skeletonAnimator.GetBool(parameter.name))
                    {
                        parameterName = parameter.name;
                    }
                }

            }
            return parameterName;
        }
        /// <summary>
        /// 애니메이션이 실행중이면, true를 반환합니다.
        /// </summary>
        /// <param name="stateName">animator에 넣은 Animation 이름을 넣어야 합니다.</param>
        /// <returns></returns>
        protected bool IsAnimationPlaying(string stateName)
        {
            AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
            bool isAnimationPlaying = currentState.IsName(stateName);
            return isAnimationPlaying;
        }

    }
}

