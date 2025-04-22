using UnityEngine;
using System.Collections.Generic;

public class PlayerStateMachine : MonoBehaviour
{

    [Header("Assign Possible States Here")]
    public List<StateSO> states;

    private StateSO currentState;
    [SerializeField]private PlayerController player;

    private void Start()
    {
        ChangeState(states[0]);
    }

    public void ChangeState(StateSO newState)
    {
        currentState?.Exit();
        currentState = newState;

        if (currentState is PlayerMoveStateSO moveState)
            moveState.player = player;

        currentState?.Enter();
    }

    private void Update()
    {
        currentState?.StateUpdate();
    }
}

