using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchCollider : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 normalColliderCenter;
    private float normalColliderHeight;

    PlayerController playerInput;

    public float crouchingColliderHeight;
    public float crouchingColliderCenterOffset;

    public bool isCrouch;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerController();
        characterController = GetComponent<CharacterController>();
        normalColliderCenter = characterController.center;
        normalColliderHeight = characterController.height;
        playerInput.CharacterControl.Crouch.started += onCrouch;
        playerInput.CharacterControl.Crouch.canceled += onCrouch;

    }

    // Update is called once per frame
    void Update()
    {
        if (isCrouch)
        {
            characterController.center = new Vector3(normalColliderCenter.x, crouchingColliderCenterOffset, normalColliderCenter.z);
            characterController.height = crouchingColliderHeight;
        }
        else
        {
            characterController.center = normalColliderCenter;
            characterController.height = normalColliderHeight;
        }
    }

    void onCrouch(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            isCrouch = !isCrouch; // Cambiará en estado verdadero o falso cuando se oprima el botón
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
