using UnityEngine;

public class CloseButtonController : MonoBehaviour
{
    public GameObject inventoryCanvas; 

    public void CloseInventory()
    {
        Debug.Log("CloseButtonController: CloseInventory method called");
        inventoryCanvas.SetActive(false); 
        Time.timeScale = 1f; 
    }
}
