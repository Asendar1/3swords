using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 9f;

    private Vector2 moveInput;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController controller;

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
    }

	private void OnDisable()
	{
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
	}

	void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        controller.Move(move * moveSpeed * Time.deltaTime);
	}

	private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
