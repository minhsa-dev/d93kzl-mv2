using UnityEngine;

public abstract class StateSO : ScriptableObject, IState
{
    [Header("State Settings")]
    [Tooltip("Optional: Name for debugging.")]
    public string stateName;

    public abstract void Enter();


    public abstract void Exit();


    public abstract void StateUpdate();



}
