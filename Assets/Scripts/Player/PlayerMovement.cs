using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // PlayerMovement son los controles de movimiento del jugador "Tanque"

    PlayerController playerInput;
    CharacterController characterController;
    Animator animator;

    [SerializeField]
    private float gravityMultiplier = 2f;

    Vector2 currentMovementInput;
    Vector3 currentRunMovement;

    public bool isMovementPressed;
    public bool isRunPressed;
    public bool isCrouch;
    public bool isTurn180;

    float rotationFactorPerFrame = 150f;
    float ySpeed;
    public float stickMagnitude;

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
        playerInput.CharacterControl.Turn.started += onTurn180;
        playerInput.CharacterControl.Turn.canceled += onTurn180;
    }

    void Update()
    {
        Vector2 movement = currentMovementInput;
        Vector3 movementDirection = new Vector3(0, 0, movement.y); // Leerá solo los valores vertiales, es decir, adelante y atrás
        // Determinará el valor de inclinación de la palanca, en botones será de 1
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        stickMagnitude = inputMagnitude; // Referencia visual en el editor de Unity para saber la mignitud de la palanca

        movementDirection.y = 0f;
        movementDirection.Normalize();

        handleRotation(); // Animación de rotar del jugador cuando se mueva horizontalmente

        float gravity = Physics.gravity.y * gravityMultiplier;

        ySpeed += gravity * Time.deltaTime;

        if (!isCrouch) // Está caminando
        {
            if (movement.y > 0.25f) // Si el movimiento de la palanca o botón es mayor a ese valor irá hacia adelante
            {
                animator.SetBool("isMoving", true); // Se activa la animación de moverse hacia adelante

                if (isRunPressed)
                {
                    // Cuando el botón de correr se activa este cambia a un valor de 2
                    animator.SetFloat("Input Magnitude", 2, 0.1f, Time.deltaTime);
                }
                else
                {
                    // Este es el valor de caminar y varia dependiendo de la magnitud de la palanca
                    animator.SetFloat("Input Magnitude", inputMagnitude, 0.2f, Time.deltaTime);
                }
            }
            else if (movement.y < -0.25f) // Si el movimiento es menor a ese valor el jugador caminará hacia atrás
            {
                animator.SetBool("isMoveBack", true); // Se activa la animación de moverse atras
                if (isRunPressed)
                {
                    animator.SetFloat("Input Magnitude", 2, 0.1f, Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
                }
            }
            else if (movementDirection.magnitude < 0.25f || movementDirection.magnitude > -0.25f) // Se desactiva el movimiento si es menor o mayor a estos valores
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isMoveBack", false);
            }
            // Se desactivan estas booleanas si es el caso de que estos esten activadas
            animator.SetBool("isCrouch", false);
            animator.SetBool("isMovingCrouch", false);
            animator.SetBool("isCrouchBackward", false);
        }
        else // Está agachado y mantiene mismos valores que el de caminado, pero cambiando las animaciones
        {
            animator.SetBool("isCrouch", true);
            if (movement.y > 0.25f)
            {
                animator.SetBool("isMovingCrouch", true);

                if (isRunPressed)
                {
                    animator.SetFloat("Input Magnitude", 2, 0.1f, Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
                }
            }
            else if (movement.y < -0.25f)
            {
                animator.SetBool("isCrouchBackward", true);
                if (isRunPressed)
                {
                    animator.SetFloat("Input Magnitude", 2, 0.1f, Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
                }
            }
            else if (movementDirection.magnitude < 0.25f || movementDirection.magnitude > -0.25f)
            {
                animator.SetBool("isMovingCrouch", false);
                animator.SetBool("isCrouchBackward", false);
            }
        }
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton(); // Leerá el valor como verdadero si el botón está presionado
    }

    void onCrouch(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            isCrouch = !isCrouch; // Cambiará en estado verdadero o falso cuando se oprima el botón
        }
    }

    void handleRotation()
    {
        Vector2 movement = currentMovementInput;
        float horizontalMovement = movement.x;
        float verticalMovement = movement.y;

        if (Mathf.Abs(horizontalMovement) < 0.5f || Mathf.Abs(verticalMovement) > 0.25f)
        {
            // Rotara junto con la animación de caminar si el jugador va en diagonal
            transform.Rotate(Vector3.up, horizontalMovement * rotationFactorPerFrame * Time.deltaTime);
        }

        if (Mathf.Abs(verticalMovement) < 0.25f && Mathf.Abs(horizontalMovement) > 0.1f && !isRunPressed)
        {
            // Si el jugador se mueve únicamente de forma horizontal se activará la animación de rotar
            if (horizontalMovement < -0.1f)
            {
                animator.SetBool("isTurnL", true); // Rotal a la izquierda
            }
            else if (horizontalMovement > 0.1f)
            {
                animator.SetBool("isTurnR", true); // Rotar a la derecha
            }

        }
        else
        {
            // Desactivarlas cuando deje de mover la palanca horizontalmente
            animator.SetBool("isTurnR", false);
            animator.SetBool("isTurnL", false);
        }
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>(); // Lee los valores del input o de la palanca
    }

    void onTurn180(InputAction.CallbackContext context)
    {
        isTurn180 = context.ReadValueAsButton();

        if (isTurn180 && stickMagnitude > 0 && !isRunPressed)
        {
            animator.SetBool("isTurn180", true);
        }
        else if (isTurn180 && isRunPressed)
        {
            animator.SetBool("isTurnRun", true);
        }
        else if (isTurn180 && stickMagnitude == 0)
        {
            animator.SetBool("isTurnIdle", true);
        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        // Wait until the current state is no longer "Turn180"
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Turn180"))
        {
            yield return null;
        }

        // Animation has ended, set the boolean to false
        animator.SetBool("isTurn180", false);
        animator.SetBool("isTurnRun", false);
        animator.SetBool("isTurnIdle", false);
    }

    void handleAnimation()
    {
        Vector3 velocity = animator.deltaPosition;
        velocity.y = ySpeed * Time.deltaTime;

        characterController.Move(velocity);
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