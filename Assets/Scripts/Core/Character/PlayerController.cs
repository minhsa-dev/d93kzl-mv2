using Animancer;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimancerComponent))]
public class PlayerController : MonoBehaviour
{


    [Header("Components")]
    [SerializeField] public CharacterController characterController { get; private set; }

    [Header("Animation")]
    [SerializeField] public AnimancerComponent Animancer { get; private set; }

    [Header("Movement")]
    public Vector2 moveInput { get; private set; }
    private Vector3 moveAccumulator = Vector3.zero;

    [Header("Air Control")]
    // acceleration when in the air (units/sec^2)
    [SerializeField] private float airControlAcceleration = 20f;
    private Vector3 horizontalVelocity = Vector3.zero;

    // current horizontal veloccity preserved across states
    public Vector3 HorizontalVelocity => horizontalVelocity;

    [Header("Gravity/Jump Settings")]
    // current vertical speed units/sec
    public float verticalVelocity;
    // upward speed applied when jump starts
    public float jumpSpeed = 5f;

    [Header("Coyote Time")]
    // how long to keep a jump press buffered
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.15f;

    //internal timers
    private float lastJumpPressedTime = float.NegativeInfinity;
    private float lastGroundedTime = float.NegativeInfinity;



    [Header("Camera")]
    private Transform playerCameraTransform;


    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (Animancer == null)
        {
            Animancer = GetComponent<AnimancerComponent>();
        }


        if (playerCameraTransform == null && Camera.main != null)
        {
            playerCameraTransform = Camera.main.transform;
        }


        Debug.Log("[Sanity] PlayerController Awake");

    }

    #region Jump
    public bool ShouldPerformJump => (Time.time - lastJumpPressedTime) <= jumpBufferTime && (Time.time - lastGroundedTime) <= coyoteTime;
    public void BufferJumpInput()
    {
        lastJumpPressedTime = Time.time;
    }
    // clears buffered jump input so we don't immediate rejump
    public void ClearJumpBuffer()
    {
        lastJumpPressedTime = float.NegativeInfinity;
    }
    #endregion
    #region Air Control
    public void SetHorizontalVelocity(Vector3 velocity)
    {
        horizontalVelocity = velocity;
    }

    public void AddHorizontalVelocity(Vector3 targetVelocity, float deltaTime)
    {
        // Steers your horizontal velocity toward the target (used in air)
        // Accelerate towards desired velocity
        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, airControlAcceleration * deltaTime);
        // never exceed maximum (targetVelocity.magnitude)
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, targetVelocity.magnitude);
    }
    #endregion
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤ COYOTE & BUFFER LOGIC ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
        // Track last time we were grounded
        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
        }
        // (States will poll this property to see if we should jump)
        // We clear the buffered jump as soon as it's consumed by the FSM.


        // Gravity: increase downward velocity
        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        if (IsGrounded() && verticalVelocity < 0f)
        {
            // If we are grounded, reset vertical velocity to zero
            verticalVelocity = 0f;
        }


        Debug.Log($"{verticalVelocity} = verticalVelocity");


        // Apply gravity displacement
        AccumulateMovement(new Vector3(0, verticalVelocity, 0), Time.deltaTime);

        // 3) Apply all accumulated movement (including horizontal & vertical) in one Move call
        MoveCharacterOncePerFrame();
    }

    private void MoveCharacterOncePerFrame()
    {
        // Allows us to accumulate movement over multiple frames and apply it once per frame

        if (moveAccumulator.sqrMagnitude > 0f)
        {
            characterController.Move(moveAccumulator);
            moveAccumulator = Vector3.zero;
        }
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void AccumulateMovement(Vector3 velocity, float deltaTime)
    {
        moveAccumulator += velocity * deltaTime;
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

    /// <summary>
    /// Returns true if the character is touching ground. Uses a small sphere cast beneath the player.
    /// </summary>

    public bool IsGrounded() => characterController.isGrounded;
}
