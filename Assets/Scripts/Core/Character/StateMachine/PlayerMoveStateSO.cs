using Animancer;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;


[CreateAssetMenu(menuName = "State/PlayerMoveStateSO")]
public class PlayerMoveStateSO : StateSO
{

    [Header("Movement Settings")]
    [Tooltip("units per second")]
    public float moveSpeed = 5f;
    public float rotationSmoothingSpeed = 10f;

    [Header("Animation Settings")]
    [SerializeField] AnimationClip moveAnimationClip;

    // A private field to store our event sequence per-instance (won¡¯t survive domain reload)
    private AnimancerEvent.Sequence moveEvents;

    [Tooltip("Fade Duration")]
    [SerializeField] private float fadeDuration = 0.1f;

    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log($"[FSM] Entering Move State ({stateName}");
    }

    public override void StateUpdate(PlayerStateMachine stateMachine, float tr)
    {
        var characterController = stateMachine.PlayerController.characterController;

        // Turn 2D input into world-space vector


        Vector3 dir = stateMachine.PlayerController.WorldMovementDirection;

        // accumlate the movement for the character controller to move once per update
        stateMachine.PlayerController.AccumulateMovement(dir * moveSpeed, tr);

        // ROTATION:
        if (dir.sqrMagnitude > stateMachine.MinimumMovementThreshold)
        {
            // current rotation
            Quaternion currentRotation = stateMachine.PlayerController.transform.rotation;
            // target rotation
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            // compute smoothing fraction: fixed percentage of remaining angle each second
            // Multiplied by tr(1 / 60s) to get per©\tick fraction, then clamped[0, 1]
            // rotationSmoothingSpeed controls how quickly you ease into the new direction (higher = snappier).
            // Multiplying by tr(1 / 60) turns that per - second rate into a per-tick fraction, so your smoothing works correctly at 60 Hz.
            float t = Mathf.Clamp01(rotationSmoothingSpeed * tr);

            // 4) Spherically interpolate toward the target by fraction t
            // This yields exponential©\decay behavior: fast initially, then eases out |       |    |  | |||
            Quaternion next = Quaternion.Slerp(currentRotation, targetRotation, t);

            stateMachine.PlayerController.transform.rotation = next;

        }




        var animState = stateMachine.PlayerController.Animancer.Play(moveAnimationClip, fadeDuration);

        if (animState.Events(this, out moveEvents))
        {
            moveEvents.OnEnd = () =>
                Debug.Log("Idle animation looped");
        }
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
