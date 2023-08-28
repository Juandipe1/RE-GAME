using UnityEngine;

public class CloseButtonController : MonoBehaviour
{
    public GameObject inventoryCanvas; // Asigna el Canvas del menú de inventario desde el Inspector

    public void CloseInventory()
    {
        Debug.Log("CloseButtonController: CloseInventory method called");
        inventoryCanvas.SetActive(false); // Ocultar el menú de inventario
        Time.timeScale = 1f; // Reanudar el juego
    }
}
