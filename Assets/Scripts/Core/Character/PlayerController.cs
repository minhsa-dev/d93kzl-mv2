using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{


    [Header("Components")]
    [SerializeField] public CharacterController characterController { get; private set; }


    [Header("Movement")]
    public Vector2 moveInput { get; private set; }


    [Header("Camera")]
    private Transform playerCameraTransform;


    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }



        if (playerCameraTransform == null && Camera.main != null)
        {
            playerCameraTransform = Camera.main.transform;
        }


        Debug.Log("[Sanity] PlayerController Awake");

    }

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
    
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
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
