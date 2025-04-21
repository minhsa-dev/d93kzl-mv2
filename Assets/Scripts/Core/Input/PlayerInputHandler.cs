using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, InputActions.IPlayerActions
{

    InputActions inputActions;
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private float movementThreshold = 0.1f;


    public void OnCrouch(InputAction.CallbackContext context)
    {
        //
    }

    public void OnJump(InputAction.CallbackContext context)
    {

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        if (dir.magnitude > movementThreshold && context.performed)
        {
            Debug.Log("Move started: " + dir);
            playerStateMachine.ChangeState(playerStateMachine.states[2]);
        } else if (dir.magnitude < movementThreshold || context.canceled)
        {
            Debug.Log("Move stopped: " + dir);
            playerStateMachine.ChangeState(playerStateMachine.states[0]);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //
    }

    void Awake()
    {
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
