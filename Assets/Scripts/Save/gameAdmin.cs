using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class gameAdmin : MonoBehaviour
{
    public DataJSON myData = new DataJSON();
    public CharacterController characterController;
    public Vector3 position;
    public GameObject player;
    public string filePat;
    PauseMenu pauseMenu;

    void Awake()
    {
        filePat = Application.streamingAssetsPath + "/" + "data.json";

        player = GameObject.FindGameObjectWithTag("Player");
        characterController = GetComponent<CharacterController>();
        characterController = FindObjectOfType<CharacterController>();
    }

    public void LoadPlayerData()
    {
        if (File.Exists(filePat))
        {
            string s = File.ReadAllText(filePat);
            myData = JsonUtility.FromJson<DataJSON>(s);
            Debug.Log("Loaded data, going to:" + s);
            characterController.enabled = false;
            player.transform.position = myData.playerPosition;
            characterController.enabled = true;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }

    public void SavePlayerData()
    {
        DataJSON newData = new DataJSON()
        {
            playerPosition = player.transform.position
        };

        string s = JsonUtility.ToJson(newData);
        Debug.Log("Save data:" + s);
        File.WriteAllText(filePat, s);
    }
}
