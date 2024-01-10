using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Walk Animation�� ����Ǵ� ����, TreeEntController ������Ʈ�� ������� �Ǵ� �Լ��� ������ �����ϴ� Ŭ���� �Դϴ�.
/// </summary>
/// <param name="animator"></param>
/// <param name="stateInfo"></param>
/// <param name="layerIndex"></param>
public class TreeEntWalk : StateMachineBehaviour
{
    private IState rootState;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<TreeEntController>().rootState;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState.Update(Time.fixedDeltaTime);
    }


}
