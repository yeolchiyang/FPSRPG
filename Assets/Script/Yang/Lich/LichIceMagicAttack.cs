using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichIceMagicAttack : StateMachineBehaviour
{
    private IState rootState;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<LichController>().rootState;
        rootState.ChangeState(LichState.Run.ToString());
        animator.GetComponent<LichController>().ExecuteIceMagicAttack();
    }
}
