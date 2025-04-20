using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, InputActions.IPlayerActions
{

    InputActions inputActions;
    [SerializeField] private PlayerStateMachine playerStateMachine;


    public void OnCrouch(InputAction.CallbackContext context)
    {
        //
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerStateMachine.ChangeState(playerStateMachine.states[1]);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //
    }

    public void OnMove(InputAction.CallbackContext context)
    {

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
