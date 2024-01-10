using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yang;

/// <summary>
/// Idle Animation�� ����Ǵ� ����, TreeEntController ������Ʈ�� ������� �Ǵ� �Լ��� ������ �����ϴ� Ŭ���� �Դϴ�.
/// </summary>
/// <param name="animator"></param>
/// <param name="stateInfo"></param>
/// <param name="layerIndex"></param>
public class TreeEntIdle : StateMachineBehaviour
{
    private IState rootState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState = animator.GetComponent<TreeEntController>().rootState;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rootState.Update(Time.fixedDeltaTime);
    }


}
