using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSG;

public class TreeEntEnranged : StateMachineBehaviour
{
    private IState rootState;
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<TreeEntController>().rootState;
        rootState.ChangeState(TreeState.EnrangedIdle.ToString());
    }


}
