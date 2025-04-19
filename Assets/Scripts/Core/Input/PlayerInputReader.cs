using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, InputActions.IPlayerActions
{
    [SerializeField] private Vector2ChannelSO moveEvent;


    InputActions controls;


    public void OnCrouch(InputAction.CallbackContext context)
    {
        //
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveEvent.RaiseEvent(context.ReadValue<Vector2>());
        } 
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //
    }

    void Awake()
    {
        controls = new InputActions();
        controls.Player.SetCallbacks(this);
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
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
