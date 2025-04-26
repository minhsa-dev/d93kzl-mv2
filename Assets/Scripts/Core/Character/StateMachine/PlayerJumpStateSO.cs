using UnityEngine;

[CreateAssetMenu(fileName = "PlayerJumpStateSO", menuName = "State/PlayerJumpStateSO")]
public class PlayerJumpStateSO : StateSO
{

    [Header("Jump Settings")]
    [SerializeField] private float jumpDuration = 0.5f; // how long to stay in jump for testing
    private float timer;

    [Header("Animancer Settings")]
    [SerializeField] AnimationClip jumpAnimationClip;
    [SerializeField] float fadeDuration = 0.1f;

    public override void Enter(PlayerStateMachine stateMachine, float tr)
    {
        timer = 0f;
        Debug.Log($"[FSM] Entering ({stateName}");
    }
    public override void StateUpdate(PlayerStateMachine stateMachine, float tr)
    {
        timer += tr;
        if (timer >= jumpDuration)
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
