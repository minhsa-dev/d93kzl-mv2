using UnityEngine;



/// <summary>
/// Lightweight wrapped around StateSO that holds per actor mutable state
/// Keeps ScriptableObjects pure data and supports multiple actors
/// </summary>
/// 

public class StateInstance : IState
{


    // reference to the state SO that this instance is based on
    protected readonly StateSO template;

    //TODO: Add mutable fields here(e.g. combo counters, timers)

    public StateInstance(StateSO template)
    {
        this.template = template;
    }

    // Same three FSM callbacks as before, but now virtual.
    public virtual void Enter(PlayerStateMachine stateMachine, float tick)
    {
        // Delegate to SO for any Setup logic
        template.Enter(stateMachine, tick);
    }


    public virtual void StateUpdate(PlayerStateMachine stateMachine, float tick)
    {
        template.StateUpdate(stateMachine, tick);
    }

    public virtual void Exit(PlayerStateMachine stateMachine, float tick)
    {
        template.Exit(stateMachine, tick);
    }
}
