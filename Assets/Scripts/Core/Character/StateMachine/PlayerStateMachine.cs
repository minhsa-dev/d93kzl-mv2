using UnityEngine;
using System.Collections.Generic;


// Drives the 60Hz fixed-timestep logic and applies FSM transitions.
public class PlayerStateMachine : MonoBehaviour
{

    [Header("Assign SO Templates(pure data)")]
    public PlayerIdleStateSO IdleStateSO;
    public PlayerMoveStateSO MoveStateSO;
    public PlayerJumpStateSO JumpStateSO;
    public PlayerFallingStateSO FallingStateSO;
    public PlayerLandingStateSO LandingStateSO;

    [Header("Runtime State Instances to reuse")]
    private IState idleStateInstance;
    private IState moveStateInstance;
    private IState jumpStateInstance;
    private IState fallingStateInstance;
    private IState landingStateInstance;    
    private IState currentState;

    [Header("Runtime State Instance Properties")]
    public IState IdleStateInstance => idleStateInstance;
    public IState MoveStateInstance => moveStateInstance;
    public IState JumpStateInstance => jumpStateInstance;
    public IState FallingStateInstance => fallingStateInstance;
    public IState LandingStateInstance => landingStateInstance;

    [Header("Settings")]
    public float MinimumMovementThreshold = 0.1f;
    const float TickRate = 1/60f; // 60 Ticks Per Second Logic
    float TickTimer;

    [Header("Buffered Input")]
    public List<BufferedInput> BufferedInputs { get; private set; } = new();

    [Header("Components")]
    [HideInInspector] public PlayerController PlayerController;
    [SerializeField] private PlayerInputHandler playerInputHandler;

    private void Awake()
    {
        // Cache PlayerController
        if (PlayerController == null)
        {
            PlayerController = GetComponent<PlayerController>();
        }

        if (playerInputHandler == null)
        {
            playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        // Instantiate per-actor state instances
        idleStateInstance = new StateInstance(IdleStateSO);
        moveStateInstance = new StateInstance(MoveStateSO);
        jumpStateInstance = new StateInstance(JumpStateSO);
        fallingStateInstance = new StateInstance(FallingStateSO);
        landingStateInstance = new StateInstance(LandingStateSO);
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

    /// <summary>
    /// Accumulates real-time into fixed 60Hz logic ticks and processes them.
    /// Consumes buffered inputs just before each tick for deterministic FSM sampling.
    /// </summary>

    private void SixtyFPSLogicTimer()
    {
        // 1) Ask Unity: ¡°how many real seconds have passed since last Update?¡±
        // 2) Bank that time into our accumulator
        TickTimer += Time.deltaTime;

        // 3) As long as we¡¯ve banked at least one TickRate (¡Ö0.01667 s),
        //    we ¡°spend¡± it on one logic update:
        int maxSteps = 5; // prevents spiral of death

        // Run fixed-rate ticks
        while (TickTimer >= TickRate && maxSteps-- > 0)
        {
            // 1.) Consume buffered inputs up to this moment
            float tickTime = Time.time;
            BufferedInputs = playerInputHandler.ConsumeBufferedInputs(tickTime);

            var stateBefore = currentState;
            // a) Perform exactly one 60 Hz logic step
            currentState?.StateUpdate(this, TickRate);

            // b) Remove that slice of time from our bank
            TickTimer -= TickRate;

            if (!ReferenceEquals(stateBefore, currentState))
                break;
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

