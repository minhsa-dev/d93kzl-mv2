

public interface IState
{
    /// <summary>
    /// Called when entering the state.
    /// use StateMachine to access the PlayerController, Animancer, etc.
    /// </summary>
    void Enter(PlayerStateMachine stateMachine, float tr);

    /// <summary>
    /// Called every tick (we'll drive this at 60Hz).
    /// </summary>
   
    void StateUpdate(PlayerStateMachine stateMachine, float tr);

    /// <summary>
    /// Called when exiting the state.
    /// </summary>
    void Exit(PlayerStateMachine stateMachine, float tr);
}
