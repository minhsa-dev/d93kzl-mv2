using UnityEngine;
using System.Collections.Generic;

public class PlayerStateMachine : MonoBehaviour
{

    [Header("Assign Possible States Here")]
    public List<StateSO> states;
    private StateSO currentState;

    private void Start()
    {
        ChangeState(states[0]);
    }

    public void ChangeState(StateSO newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    private void Update()
    {
        currentState?.StateUpdate();
    }
}

