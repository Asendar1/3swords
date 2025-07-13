using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{

    [SerializeField] private CinemachineCamera mainCamera;

    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -14f;
    [SerializeField] private Vector3 velocity;

    private Vector2 moveInput;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Jump"].started += OnJump;
        playerInput.actions["Sprint"].performed += OnSprint;
        playerInput.actions["Sprint"].canceled += OnSprint;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Jump"].started -= OnJump;
    }

    void Update()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 move = (cameraRight * moveInput.x + cameraForward * moveInput.y).normalized;

        if (move != Vector3.zero)
        {
            Quaternion look = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, 10f * Time.deltaTime);
        }

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (velocity.y < 0 && controller.isGrounded)
        {
            velocity.y = -2f;
        }
        if (velocity.y > 0)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * 2f * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded) return;

        velocity.y = jumpForce;
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            moveSpeed += sprintSpeed;
        }
        if (context.canceled)
        {
            moveSpeed -= sprintSpeed;
        }
    }
}
