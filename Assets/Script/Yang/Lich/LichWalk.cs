using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichWalk : StateMachineBehaviour
{
    private IState rootState;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<LichController>().rootState;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Walk update ½ÇÇàµÊ");
        rootState.Update(Time.fixedDeltaTime);
    }
}
