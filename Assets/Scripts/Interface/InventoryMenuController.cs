using UnityEngine;

public class InventoryMenuController : MonoBehaviour
{
    public GameObject inventoryCanvas; 

    void Start()
    {
        inventoryCanvas.SetActive(false); 
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
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf); 

        if (inventoryCanvas.activeSelf)
        {
            Time.timeScale = 0f; 
        }
        else
        {
            Time.timeScale = 1f; 
        }
    }
}
