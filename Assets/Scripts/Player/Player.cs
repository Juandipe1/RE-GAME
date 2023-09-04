using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerController playerInput;
    CharacterController characterController;

    public Vector2 currentMovementInput;

    public bool isMovementPressed;
    public bool isRunPressed;
    public bool isCrouch;
    public bool roofExists;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerController();
        characterController = GetComponent<CharacterController>();

        playerInput.CharacterControl.Movement.started += onMovementInput;
        playerInput.CharacterControl.Movement.canceled += onMovementInput;
        playerInput.CharacterControl.Movement.performed += onMovementInput;
        playerInput.CharacterControl.Run.started += onRun;
        playerInput.CharacterControl.Run.canceled += onRun;
        playerInput.CharacterControl.Crouch.started += onCrouch;
        playerInput.CharacterControl.Crouch.canceled += onCrouch;
        roofExists = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
    }

    public void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    public void onCrouch(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {

            if (!roofExists)
            {
                isCrouch = !isCrouch;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DontCrouch"))
        {
            roofExists = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DontCrouch"))
        {
            roofExists = false;
        }
    }
    private void OnEnable()
    {
        playerInput.CharacterControl.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControl.Disable();
    }
}
