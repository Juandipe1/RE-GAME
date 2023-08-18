using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private PlayerController playerController;
    private InputAction menu;

    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        playerController = new PlayerController();
        DesactivateMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        menu = playerController.Menu.Pause;
        menu.Enable();

        menu.performed += Pause;
    }

    private void OnDisable()
    {
        menu.Disable();
    }

    void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DesactivateMenu();
        }
    }

    void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseUI.SetActive(true);
    }

    public void DesactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseUI.SetActive(false);
        isPaused = false;
    }
}
