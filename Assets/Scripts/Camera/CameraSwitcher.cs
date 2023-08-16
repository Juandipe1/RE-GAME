using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;
    public Transform player;
    public float switchDistance = 10f;

    public Camera activeCamera;

    private void Start()
    {
        // Desactivar todas las cámaras excepto la primera en la lista
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        activeCamera = cameras[0];
    }

    private void Update()
    {
        // Comprobar si alguna cámara está dentro del rango de activación
        Camera closestCamera = null;
        float closestDistance = Mathf.Infinity;

        foreach (Camera camera in cameras)
        {
            float distance = Vector3.Distance(player.position, camera.transform.position);

            if (distance <= switchDistance && distance < closestDistance)
            {
                closestCamera = camera;
                closestDistance = distance;
            }
        }

        // Activar la cámara más cercana y desactivar las demás
        for (int i = 0; i < cameras.Length; i++)
        {
            bool isActive = (cameras[i] == closestCamera);
            cameras[i].gameObject.SetActive(isActive);
        }

        activeCamera = closestCamera;
    }

    private void OnDrawGizmos()
    {
        // Dibujar un gizmo para cada cámara en la escena
        foreach (Camera camera in cameras)
        {
            Gizmos.color = (camera == activeCamera) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(camera.transform.position, switchDistance);
        }
    }

    private void DrawWireSphere(Vector3 position, float radius)
    {
        Gizmos.DrawWireSphere(position, radius);
    }
}
