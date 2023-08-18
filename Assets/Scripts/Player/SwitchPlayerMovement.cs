using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerMovemet : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerMovement2 playerMovement2;

    bool ScriptSwitch;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement2 = GetComponent<PlayerMovement2>();

        playerMovement.enabled = true;
        playerMovement2.enabled = false;
    }

    // Update is called once per frame
    public void ToggleScripts()
    {
        if (ScriptSwitch)
        {
            playerMovement.enabled = true;
            playerMovement2.enabled = false;
            print("Movimiento del tanque activado");
        }
        else
        {
            playerMovement.enabled = false;
            playerMovement2.enabled = true;
            print("Movimiento del moderno activado");
        }

        ScriptSwitch = !ScriptSwitch;
    }
}
