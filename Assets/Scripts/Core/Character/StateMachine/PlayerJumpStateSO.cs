using UnityEngine;

[CreateAssetMenu(fileName = "PlayerJumpStateSO", menuName = "State/PlayerJumpStateSO")]
public class PlayerJumpStateSO : StateSO
{

    [Header("Jump Settings")]
    [SerializeField] private float jumpSpeed = 5f;

    [Header("Animancer Settings")]
    [SerializeField] AnimationClip jumpAnimationClip;
    [SerializeField] float fadeDuration = 0.1f;

    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        stateMachine.PlayerController.verticalVelocity = jumpSpeed;

        if (jumpAnimationClip != null)
        {
            stateMachine.PlayerController.Animancer.Play(jumpAnimationClip, fadeDuration);
        }
        Debug.Log($"[FSM] Entering ({stateName}");
    }
    public override void StateUpdate(PlayerStateMachine stateMachine, float tr)
    {
        var dir = stateMachine.PlayerController.WorldMovementDirection;
        float runSpeed = stateMachine.MoveStateSO.moveSpeed;
        stateMachine.PlayerController.AddHorizontalVelocity(dir * runSpeed, tr);
        stateMachine.PlayerController.AccumulateMovement(stateMachine.PlayerController.HorizontalVelocity, tr);



        if (stateMachine.PlayerController.IsGrounded() && stateMachine.PlayerController.verticalVelocity <= 0f)
        {
            stateMachine.ChangeState(stateMachine.IdleStateInstance);
            return;
        }
        Debug.Log($"[FSM] Updating ({stateName}");
    }

    public override void Exit(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log($"[FSM] Exiting ({stateName}");
    }

}
