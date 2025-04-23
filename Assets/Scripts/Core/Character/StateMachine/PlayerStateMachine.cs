using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{

    [Header("Assign SO Templates(pure data)")]
    public PlayerIdleStateSO IdleStateSO;
    public PlayerMoveStateSO MoveStateSO;


    // runtime state instances
    private IState idleStateInstance;
    private IState moveStateInstance;
    private IState currentState;

    // public getters so other classes(e.g. SOs) can access them
    public IState IdleStateInstance => idleStateInstance;
    public IState MoveStateInstance => moveStateInstance;

    [Header("Settings")]
    [Tooltip("Minimum input magnitude to count as movement")]
    public float MinimumMovementThreshold = 0.1f;
    const float TickRate = 1/60f; // 60FPS
    float TickTimer;

    [HideInInspector] public PlayerController PlayerController;

    private void Awake()
    {
        // Cache PlayerController
        PlayerController = GetComponent<PlayerController>();

        // Instantiate per-actor state instances
        idleStateInstance = new StateInstance(IdleStateSO);
        moveStateInstance = new StateInstance(MoveStateSO);

    }


    private void Start()
    {
       ChangeState(idleStateInstance);
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

    // modified to accept IState (wrapper) instead of raw SO
    public void ChangeState(IState newState)
    {
        currentState?.Exit(this, TickRate);
        currentState = newState;
        currentState?.Enter(this, TickRate);
    }
}

