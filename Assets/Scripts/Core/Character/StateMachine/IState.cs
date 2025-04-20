using UnityEditorInternal;
using UnityEngine;
using UnityEditor.Animations;

public interface IState
{
    void Enter();
    void StateUpdate();
    void Exit();
}
