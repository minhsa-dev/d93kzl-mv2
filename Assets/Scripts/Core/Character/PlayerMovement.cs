using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2ChannelSO moveEvent;
    [SerializeField] CharacterController characterController;
    private Vector2 moveInput;
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
        moveEvent.OnEventRaised += HandleMovement;
    }

    private void OnDisable()
    {
        moveEvent.OnEventRaised -= HandleMovement;
    }

    private void HandleMovement(Vector2 vector)
    {
        moveInput = vector;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime);
        Debug.Log($"Movement Input: {moveInput}");
    }
}
