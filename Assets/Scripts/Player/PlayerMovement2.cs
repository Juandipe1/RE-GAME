using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    // PlayerMovement2 son los controles de movimiento del jugador "Moderno"

    Player player;
    Animator animator;

    [SerializeField]
    private float rotationSpeed = 150f;

    [SerializeField]
    private float gravityMultiplier = 2f;

    [SerializeField]
    private Transform cameraTransform;

    Vector2 currentMovementInput;

    float ySpeed;
    public float stickMagnitude;

    bool isMoving = false;
    Vector3 lastCamAngle;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;

        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = player.currentMovementInput;
        Vector3 movementDirection = new Vector3(movement.x, 0, movement.y);
        // Determinará el valor de inclinación de la palanca, en botones será de 1
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude); 

        stickMagnitude = inputMagnitude; // Referencia visual en el editor de Unity para saber la mignitud de la palanca

        // Update the stored cameraRotationY only when the inputs change
        isMoving = (player.currentMovementInput != Vector2.zero);

        if (!isMoving)
        {
            lastCamAngle = new Vector3(cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
            // Guardará la última posición de la cámara en la que te quedaste quieto, cuando te muevas se mantendrá hasta que sueltes el movimiento
        }

        // Use the stored cameraRotationY for relative movement
        movementDirection = Quaternion.Euler(0, lastCamAngle.y, 0) * movementDirection;
        movementDirection.y = 0f;
        movementDirection.Normalize();

        float gravity = Physics.gravity.y * gravityMultiplier;

        ySpeed += gravity * Time.deltaTime;

        if (!player.isCrouch) // El personaje no está agachado
        {
            if (movementDirection != Vector3.zero) // Saber si el movimiento es un valor diferente de 0
            {
                animator.SetBool("isMoving", true); // Se activa la animación de moverse

                // Use Smooth Damp to smoothly rotate the character towards the desired direction
                Vector3 currentDir = transform.forward;
                Vector3 targetDir = movementDirection;
                Vector3 newDir = Vector3.RotateTowards(currentDir, targetDir, rotationSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);

                if (player.isRunPressed)
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
            else
            {
                animator.SetBool("isMoving", false); // Se desactiva para estar en idle
            }
            // Se desactivan estas booleanas si es el caso de que estos esten activadas
            animator.SetBool("isCrouch", false); 
            animator.SetBool("isMovingCrouch", false);
        }
        else // Está agachado y mantiene mismos valores que el de caminado, pero cambiando las animaciones
        {
            animator.SetBool("isCrouch", true); // Activa la animación de agacharse

            if (movementDirection != Vector3.zero)
            {
                animator.SetBool("isMovingCrouch", true); // Se mueve con la animación de agachado

                // Use Smooth Damp to smoothly rotate the character towards the desired direction
                Vector3 currentDir = transform.forward;
                Vector3 targetDir = movementDirection;
                Vector3 newDir = Vector3.RotateTowards(currentDir, targetDir, rotationSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);

                if (player.isRunPressed)
                {
                    animator.SetFloat("Input Magnitude", 2, 0.1f, Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
                }
            }
            else
            {
                animator.SetBool("isMovingCrouch", false);
            }
        }
    }
}
