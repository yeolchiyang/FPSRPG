using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichEnranged : StateMachineBehaviour
{
    private IState rootState;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<LichController>().IsInvulnerable = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<LichController>().rootState;
        rootState.ChangeState(LichState.EnrangedIdle.ToString());
        animator.GetComponent<LichController>().IsInvulnerable = false;
        Debug.Log("LichEnranged 상태를 벗어남");
    }

}
