using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;


[CreateAssetMenu(menuName = "State/PlayerMoveStateSO")]
public class PlayerMoveStateSO : StateSO
{

    [Header("Movement Settings")]
    [Tooltip("units per second")]
    public float moveSpeed = 20f;


    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log($"[FSM] Entering Move State ({stateName}");
    }

    public override void StateUpdate(PlayerStateMachine stateMachine, float tr)
    {
        var characterController = stateMachine.PlayerController.characterController;

        // Turn 2D input into world-space vector

        Vector3 dir = stateMachine.PlayerController.WorldMovementDirection;

        // Move character controller

        characterController.Move(dir * moveSpeed * tr);

        // If no input, switch to idle state

        if (stateMachine.PlayerController.moveInput.magnitude < stateMachine.MinimumMovementThreshold)
        {
            stateMachine.ChangeState(stateMachine.IdleStateInstance);
        }

        Debug.Log($"[FSM] Updating Move State ({stateName}) ");
    }

    public override void Exit(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log($"[FSM] Exiting Move State ({stateName})");
    }


}
