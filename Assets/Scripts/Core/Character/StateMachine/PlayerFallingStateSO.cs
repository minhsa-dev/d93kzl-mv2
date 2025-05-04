using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFallingStateSO", menuName = "State/PlayerFallingStateSO")]
public class PlayerFallingStateSO : StateSO
{
    [Header("Animancer Settings")]
    [SerializeField] private AnimationClip fallAnimationClip;
    [SerializeField] private float fadeDuration = 0.1f;
    [SerializeField] private float rotationSmoothingSpeed = 10f;

    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        if (fallAnimationClip != null)
        {
            stateMachine.PlayerController.Animancer.Play(fallAnimationClip, fadeDuration);
            Debug.Log($"[FSM] Entering Fall State");
        }
    }

    public override void StateUpdate(PlayerStateMachine stateMachine, float tr)
    {
        var dir = stateMachine.PlayerController.WorldMovementDirection;
        float runSpeed = stateMachine.MoveStateSO.moveSpeed;
        stateMachine.PlayerController.AddHorizontalVelocity(dir * runSpeed, tr);
        stateMachine.PlayerController.AccumulateMovement(stateMachine.PlayerController.HorizontalVelocity, tr);

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


            if (stateMachine.PlayerController.IsGrounded())
            {
                stateMachine.ChangeState(stateMachine.LandingStateInstance);
                return;
            }
            Debug.Log($"[FSM] Updating Fall State");
        }
    }

    public override void Exit(PlayerStateMachine stateMachine, float tr)
    {
        Debug.Log($"[FSM] Exiting Fall State");
    }

}
