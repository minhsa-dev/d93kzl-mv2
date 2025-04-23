using UnityEngine;

[CreateAssetMenu(menuName = "State/IdleStateSO")]
public class PlayerIdleStateSO : StateSO
{
    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log("Entering Idle State");
    }

    public override void StateUpdate(PlayerStateMachine stateMachine, float tr)
    {

        if (stateMachine.PlayerController.moveInput.magnitude > stateMachine.MinimumMovementThreshold)
        {
            stateMachine.ChangeState(stateMachine.MoveStateInstance);
        }
        Debug.Log("Updating Idle State");
    }

    public override void Exit(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log("Exiting Idle State");
    }


}
