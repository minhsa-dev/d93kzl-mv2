using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, InputActions.IPlayerActions
{


    [Header("Components")]
    [SerializeField] public CharacterController characterController { get; private set; }
    [SerializeField] private InputActions inputActions;

    [Header("Movement")]
    public Vector2 moveInput { get; private set; }
    [SerializeField] private float moveSpeed = 3f;

    [Header("Camera")]
    private Transform playerCameraTransform;


    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (inputActions == null)
        {
            inputActions = new InputActions();
        }

        if (playerCameraTransform == null && Camera.main != null)
        {
            playerCameraTransform = Camera.main.transform;
        }

        inputActions.Player.SetCallbacks(this);
        Debug.Log("[Sanity] PlayerController Awake: callbacks set");

    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        Debug.Log("[Sanity] PlayerController OnEnable: input enabled");
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        Debug.Log("[Sanity] PlayerController OnDisable: input disabled");
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public Vector3 WorldMovementDirection
    {
        get
        {
            Vector3 forward = playerCameraTransform.forward;
            Vector3 right = playerCameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            return (forward * moveInput.y + right * moveInput.x).normalized;
        }
    }

    #region InputAction Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        //
    }
    #endregion
}
