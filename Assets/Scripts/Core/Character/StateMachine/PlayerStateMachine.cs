using UnityEngine;
using System.Collections;

public class PlayerStateMachine : MonoBehaviour
{

    [Header("Assign Possible States Here")]
    public PlayerIdleStateSO IdleState;
    public PlayerMoveStateSO MoveState;

    [Header("Settings")]
    [Tooltip("Minimum input magnitude to count as movement")]
    public float MinimumMovementThreshold = 0.1f;
    const float TickRate = 1/60f; // 60FPS
    float TickTimer;

    [HideInInspector] public PlayerController PlayerController;
    private StateSO currentState;

    private void Awake()
    {
        // TODO: replace with DI container in future
        PlayerController = GetComponent<PlayerController>();
    }


    private void Start()
    {
       ChangeState(IdleState);
    }


    // 60FPS
    private void Update()
    {
        SixtyFPSLogicTimer();
    }

    private void SixtyFPSLogicTimer()
    {
        // 1) Ask Unity: ¡°how many real seconds have passed since last Update?¡±
        // 2) Bank that time into our accumulator
        TickTimer += Time.deltaTime;

        // 3) As long as we¡¯ve banked at least one TickRate (¡Ö0.01667 s),
        //    we ¡°spend¡± it on one logic update:
        int maxSteps = 5;

        while (TickTimer >= TickRate && maxSteps-- > 0)
        {
            // a) Perform exactly one 60 Hz logic step
            currentState?.StateUpdate(this, TickRate);

            // b) Remove that slice of time from our bank
            TickTimer -= TickRate;
        }
    }

    public void ChangeState(StateSO newState)
    {
        currentState?.Exit(this, TickRate);
        currentState = newState;
        currentState?.Enter(this, TickRate);
    }
}

