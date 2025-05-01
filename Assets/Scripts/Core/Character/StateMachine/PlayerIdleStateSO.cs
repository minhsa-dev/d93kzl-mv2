using Animancer;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "State/IdleStateSO")]
public class PlayerIdleStateSO : StateSO
{
    [Header("Animancer Settings")]
    [SerializeField] AnimationClip idleAnimationClip;

    [Tooltip("Fade Duration")]
    [SerializeField] float fadeDuration = 0.1f;

    // A private field to store our event sequence per-instance (won¡¯t survive domain reload)
    private AnimancerEvent.Sequence _idleEvents;

    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log("Entering Idle State");
        var animState = stateMachine.PlayerController.Animancer.Play(idleAnimationClip, fadeDuration);

        if (animState.Events(this, out _idleEvents))
        {
            _idleEvents.OnEnd = () =>
                Debug.Log("Idle animation looped");
        }
    }

    public override void StateUpdate(PlayerStateMachine stateMachine, float tr)
    {

        if (stateMachine.PlayerController.moveInput.magnitude > stateMachine.MinimumMovementThreshold)
        {
            stateMachine.ChangeState(stateMachine.MoveStateInstance);
            return;
        }
        Debug.Log("Updating Idle State");

        bool jumpRequested = stateMachine.BufferedInputs.
            Any(b => b.Action == BufferedInput.ActionType.Jump);

        if (jumpRequested && stateMachine.PlayerController.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.JumpStateInstance);
            return;
        }

    }

    public override void Exit(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log("Exiting Idle State");
    }


}
