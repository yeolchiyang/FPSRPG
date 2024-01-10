using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Walk Animation이 실행되는 동안, TreeEntController 컴포넌트의 멤버변수 또는 함수를 가져와 실행하는 클래스 입니다.
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
