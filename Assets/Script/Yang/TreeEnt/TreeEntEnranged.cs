using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSG;
/// <summary>
/// �ִϸ��̼��� ������, EnrangedIdle ���°� �Ǹ�, �������°� �����˴ϴ�.
/// </summary>
public class TreeEntEnranged : StateMachineBehaviour
{
    private IState rootState;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<TreeEntController>().IsInvulnerable = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<TreeEntController>().rootState;
        rootState.ChangeState(TreeState.EnrangedIdle.ToString());
        animator.GetComponent<TreeEntController>().IsInvulnerable = false;
    }


}
