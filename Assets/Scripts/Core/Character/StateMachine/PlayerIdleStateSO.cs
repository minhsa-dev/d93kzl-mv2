using UnityEngine;

[CreateAssetMenu(menuName = "State/IdleStateSO")]
public class PlayerIdleStateSO : StateSO
{
    [Header("Animancer Settings")]
    [SerializeField] AnimationClip idleAnimationClip;

    [Tooltip("Fade Duration")]
    [SerializeField] float fadeDuration = 0.1f;


    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log("Entering Idle State");
        stateMachine.PlayerController.Animancer.Play(idleAnimationClip, fadeDuration);
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
