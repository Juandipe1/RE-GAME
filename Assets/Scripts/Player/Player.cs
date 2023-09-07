using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    PlayerController playerInput;
    public CharacterController characterController;
    public Animator animator;

    public Vector2 currentMovementInput;

    public bool isMovementPressed;
    public bool isRunPressed;
    public bool isCrouch;
    public bool roofExists;
    public bool isItemPickUp;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerController();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInput.CharacterControl.Movement.started += onMovementInput;
        playerInput.CharacterControl.Movement.canceled += onMovementInput;
        playerInput.CharacterControl.Movement.performed += onMovementInput;
        playerInput.CharacterControl.Run.started += onRun;
        playerInput.CharacterControl.Run.canceled += onRun;
        playerInput.CharacterControl.Crouch.started += onCrouch;
        playerInput.CharacterControl.Crouch.canceled += onCrouch;
        playerInput.CharacterControl.Pick.started += onPick;
        playerInput.CharacterControl.Pick.canceled += onPick;
        roofExists = false;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovementPressed == false) return;
    }

    public void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput != Vector2.zero;
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

    public void onPick(InputAction.CallbackContext context)
    {
        isItemPickUp = playerInput.CharacterControl.Pick.WasPressedThisFrame();
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
    private void OnAnimatorMove()
    {
        if (characterController.enabled)
        {
            Vector3 newPosition = transform.position + animator.deltaPosition;
            Quaternion newRotation = transform.rotation * animator.deltaRotation;

            characterController.Move(newPosition - transform.position);
            transform.rotation = newRotation;
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
