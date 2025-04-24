using Animancer;
using UnityEngine;


[CreateAssetMenu(menuName = "State/PlayerMoveStateSO")]
public class PlayerMoveStateSO : StateSO
{

    [Header("Movement Settings")]
    [Tooltip("units per second")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

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

        // ROTATION: Only rotate if we have input

            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(dir);

        // Move character controller

        characterController.Move(dir * moveSpeed * tr);


            // Smoothly rotate towards target rotation
            stateMachine.PlayerController.transform.rotation = Quaternion.RotateTowards(Quaternion.identity, targetRotation, rotationSpeed * tr);

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
