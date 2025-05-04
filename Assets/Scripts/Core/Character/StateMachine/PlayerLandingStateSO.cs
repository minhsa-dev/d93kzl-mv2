using Animancer;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLandingState", menuName = "State/PlayerLandingStateSO")]
public class PlayerLandingStateSO : StateSO
{
    [Header("Animancer Settings")]
    [SerializeField] private AnimationClip landAnimationClip;
    [SerializeField] private float fadeDuration = 0.1f;
    private AnimancerEvent.Sequence _landEvents;

    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log($"[FSM] Entering Land State");
        var animState = stateMachine.PlayerController.Animancer.Play(landAnimationClip, fadeDuration);

        // Once the landing animation finishes, transition back to Idle
        if (animState.Events(this, out _landEvents))
        {
            _landEvents.OnEnd = () =>
                stateMachine.ChangeState(stateMachine.IdleStateInstance);
        }
    }

    public override void StateUpdate(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log($"[FSM] Updating Land State");

        if (stateMachine.PlayerController.moveInput.magnitude > stateMachine.MinimumMovementThreshold)
        {
            stateMachine.ChangeState(stateMachine.MoveStateInstance);
            return;
        }
    }

    public override void Exit(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log($"[FSM] Exiting Land State");
    }

}
