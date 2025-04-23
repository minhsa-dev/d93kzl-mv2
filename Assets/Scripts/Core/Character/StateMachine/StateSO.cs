using UnityEngine;

public abstract class StateSO : ScriptableObject, IState
{
    [Header("State Settings")]
    [Tooltip("Optional: Name for debugging.")]
    public string stateName;

    public abstract void Enter(PlayerStateMachine stateMachine, float tr);

    public abstract void StateUpdate(PlayerStateMachine stateMachine, float tr);

    public abstract void Exit(PlayerStateMachine stateMachine, float tr);





}
