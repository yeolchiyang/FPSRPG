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
        [SerializeField] protected GameObject HitEffect;

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
        /// NavMesh�� ���� �Ǵ� ������մϴ�. 
        /// �������� Player ��ġ �Դϴ�.
        /// ��� ������Ʈ�� ���� �����մϴ�.
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

        /// <summary>
        /// NavMesh�� ���� �Ǵ� ������մϴ�. 
        /// �������� ���� ���� �����մϴ�. 
        /// ��� ������Ʈ�� ���� �����մϴ�.
        /// </summary>
        /// <param name="speed">1�ʸ��� speed��ŭ�� �Ÿ��� �̵��մϴ�.</param>
        /// <param name="targetPosition"></param>
        protected void StartNavigation(float speed, Vector3 targetPosition)
        {
            skeletonNav.isStopped = false;
            skeletonNav.ResetPath();//ResetPath -> SetDestination �ؾ� ���۵� �մϴ�.
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
            // ���� �ִϸ������� ��� �Ķ���͸� �����ɴϴ�.
            AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;

            // �� �Ķ���Ϳ� ���� �ݺ��մϴ�.
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                // ������ �Ķ�������� Ȯ���ϰ�, ���ܵ��� ���� ��� ���� false�� �����մϴ�.
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
        /// ����, �������� �Ծ��� �� ������ ���� state�� ��ȯ�ϱ� �����Դϴ�.
        /// </summary>
        /// <returns>������ ���°��� string ���� ��ȯ�մϴ�.</returns>
        protected string GetBoolAnimationName()
        {
            string parameterName = "";
            // ���� �ִϸ������� ��� �Ķ���͸� �����ɴϴ�.
            AnimatorControllerParameter[] parameters = skeletonAnimator.parameters;
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    //true�� �Ķ���͸�
                    if (skeletonAnimator.GetBool(parameter.name))
                    {
                        parameterName = parameter.name;
                    }
                }

            }
            return parameterName;
        }
        /// <summary>
        /// �ִϸ��̼��� �������̸�, true�� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="stateName">animator�� ���� Animation �̸��� �־�� �մϴ�.</param>
        /// <returns></returns>
        protected bool IsAnimationPlaying(string stateName)
        {
            AnimatorStateInfo currentState = skeletonAnimator.GetCurrentAnimatorStateInfo(0);
            bool isAnimationPlaying = currentState.IsName(stateName);
            return isAnimationPlaying;
        }

    }
}

