using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack Animation�� ���� ��, Walk State�� ���¸� ��ȯ�ϴ� ����� �����Ǿ� �ֽ��ϴ�.
/// </summary>
/// <param name="animator"></param>
/// <param name="stateInfo"></param>
/// <param name="layerIndex"></param>
public class TreeEntAttack : StateMachineBehaviour
{
    private IState rootState;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<TreeEntController>().rootState;
        rootState.ChangeState(TreeState.Walk.ToString());
    }

}
