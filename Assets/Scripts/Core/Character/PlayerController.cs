using Animancer;
using System;
using UnityEditor.Experimental.GraphView;
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

    [Header("Jump Helpers")]
    [Tooltip("Gravity Multipler when falling")]
    [SerializeField] private float fallGravityMultiplier = 2.5f;
    [Tooltip("Gravity Multiplier when jump button released early")]
    [SerializeField] private float lowJumpGravityMultiplier = 2.0f;
    [Tooltip("Gravity multiplier at jump apex (hang time)")]
    [SerializeField] private float apexHangGravityMultiplier = 0.5f;
    [Tooltip("Vertical-velocity threshold for apex hang")]
    [SerializeField] private float apexHangThreshold = 0.2f;

    private bool jumpHeld = false;

    // Called by input handler when jump is pressed/released
    public void SetJumpHeld(bool held)
    {
        jumpHeld = held;
    }

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

    public bool IsGrounded() => characterController.isGrounded;

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

        // ---Variable gravity
        float gScale = 1f;
        if (verticalVelocity < 0f)
        {
            gScale = fallGravityMultiplier;
        }
        else if (verticalVelocity > 0f && !jumpHeld)
        {
            gScale = lowJumpGravityMultiplier;
        }
        // Apex hang
        if (Mathf.Abs(verticalVelocity) < apexHangThreshold)
        {
            gScale = Mathf.Min(gScale, apexHangGravityMultiplier);
        }


        verticalVelocity += Physics.gravity.y * gScale * Time.deltaTime;

        // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤ COYOTE & BUFFER LOGIC ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
        // Track last time we were grounded
        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
        }
        // (States will poll this property to see if we should jump)
        // We clear the buffered jump as soon as it's consumed by the FSM.

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


}
