using UnityEngine;


[CreateAssetMenu(menuName = "State/PlayerMoveStateSO")]
public class PlayerMoveStateSO : StateSO
{

    [Header("Movement Settings")]
    [Tooltip("units per second")]
    public float moveSpeed = 4f;

    //Assigned at runtime by StateMachine.ChangeState
    [HideInInspector] public PlayerController player;

    public override void Enter()
    {
        Debug.Log($"[FSM] Entering Move State ({stateName}");
    }

    public override void StateUpdate()
    {
        // Turn 2D input into world-space vector
        Vector3 dir = player.WorldMovementDirection;
        // Move character controller
        player.characterController.Move(dir * moveSpeed * Time.deltaTime);
        Debug.Log($"[FSM] Updating Move State ({stateName}) ");
    }

    public override void Exit()
    {
        Debug.Log($"[FSM] Exiting Move State ({stateName})");
    }


}
