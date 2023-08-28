using UnityEngine;

public class InventoryMenuController : MonoBehaviour
{
    public GameObject inventoryCanvas; // Asigna el Canvas del menú de inventario desde el Inspector

    void Start()
    {
        inventoryCanvas.SetActive(false); // Inicialmente, el menú de inventario está oculto
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryMenu();
        }
    }

    void ToggleInventoryMenu()
    {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf); // Alternar la visibilidad del menú

        if (inventoryCanvas.activeSelf)
        {
            Time.timeScale = 0f; // Pausar el juego
        }
        else
        {
            Time.timeScale = 1f; // Reanudar el juego
        }
    }
}
