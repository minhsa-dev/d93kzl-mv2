using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{


    [Header("Components")]
    [SerializeField] CharacterController characterController;

    [Header("Movement")]
    private Vector3 moveInput;
    [SerializeField] private float moveSpeed = 3f;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }


    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void HandleMovement(Vector3 vector)
    {
        moveInput = vector;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
        characterController.Move(new Vector3(moveInput.x, 0, moveInput.z) * moveSpeed * Time.deltaTime);
        }

    }
}
