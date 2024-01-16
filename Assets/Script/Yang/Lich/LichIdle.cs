using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichIdle : StateMachineBehaviour
{
    private IState rootState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<LichController>().rootState;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState.Update(Time.fixedDeltaTime);
    }
}
