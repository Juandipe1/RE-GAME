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

    Player player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        normalColliderCenter = characterController.center;
        normalColliderHeight = characterController.height;

    }

    // Update is called once per frame
    void Update()
    {
        if (player.isCrouch)
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
}
