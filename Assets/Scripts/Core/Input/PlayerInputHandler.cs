using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Captures raw input callbacks, buffers them, and exposes buffered inputs for the FSM to consume.
public class PlayerInputHandler : MonoBehaviour, InputActions.IPlayerActions
{

    InputActions inputActions;
    [SerializeField] private PlayerStateMachine playerStateMachine;

    [Header("Buffer")]

    // Maximum number of buffered events to keep
    private const int BufferCapacity = 16;

    // Ring buffer to store recent input events
    private readonly Queue<BufferedInput> inputBuffer = new(BufferCapacity);

    // Time window (in seconds) to consider inputs "fresh" e.g 0.15f = 0.15 seconds
    [SerializeField] private float bufferWindow = 0.15f;


    #region Buffer Input System
    private void Enqueue(BufferedInput.ActionType action)
    {
        // Adds a new input event to the ring buffer, discarding oldest if full.
        if (inputBuffer.Count >= BufferCapacity)
        {
            inputBuffer.Dequeue();
        }

        inputBuffer.Enqueue(new BufferedInput
        {
            Action = action,
            eventTime = Time.time
        });
    }

    /// <summary>
    /// Called once per FSM tick to retrieve and clear inputs that occurred within the bufferWindow.
    /// </summary>
    /// <param name="currentTime">Current Time.time at tick.</param>
    public List<BufferedInput> ConsumeBufferedInputs(float currentTime)
    {
        var consumed = new List<BufferedInput>(BufferCapacity);

        // process all buffered inputs
        while (inputBuffer.Count > 0)
        {

            // checks the oldest event
            var next = inputBuffer.Peek();
            // if event is recent enough, consume it (latest first, >= current time - bufferwindow, 
            // any input within that time is added to the list
            if (next.eventTime >= currentTime - bufferWindow)
            {
                consumed.Add(next);
            }
                        // Dequeue all inputs to get ready for new ones
                        // should only be one call for input per logic tick
                       inputBuffer.Dequeue();

        }

        return consumed;
    }
    #endregion


    #region Callback Contexts
    public void OnCrouch(InputAction.CallbackContext context)
    {
        //NOOP for now
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // when user press jump, buffer the input
        // FSM will check buffer on its tick
        if (context.performed)
        {
            playerStateMachine.PlayerController.BufferJumpInput();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //NOOP - handled by cinemachine
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        playerStateMachine.PlayerController.SetMoveInput(dir);
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        //NOOP for now
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //NOOP for now
    }
    #endregion

    void Awake()
    {
        // Initialize InputActions and register this handler
        inputActions = new InputActions();
        inputActions.Player.SetCallbacks(this);
    }


    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
