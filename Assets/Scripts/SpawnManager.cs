using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public GameObject Player;
    private Transform defaultPoint;
    private bool setPoint;
    private string spawnLocation = "DefaultSpawnPoint";
    [SerializeField] private string setSpawnPoint = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    public void SetSpwanLocation(string _location)
    {
        spawnLocation = _location;
        setPoint = true;
    }

    private void OnLevelLoaded(int level)
    {
        print(spawnLocation);
        if (level > 1)
        {
            Debug.Log("Spawn at SpawnHere location");
            Transform temp = GameObject.Find("DefaultSpawnPoint").transform;
            Instantiate(Player, temp.position, Quaternion.identity);
            ResetLocation();
        }
        if (level == 1)
        {
            if (!setPoint)
            {
                Debug.Log("No set location - will spawn at default");
                spawnAtStart();
            }
            else
            {
                spawnAtSetLocation();
            }
        }
    }

    void spawnAtSetLocation()
    {
        Debug.Log("Done spawning at set location");
        Transform spawnPoint = GameObject.Find(setSpawnPoint).transform;
        Instantiate(Player, spawnPoint.position, spawnPoint.rotation);
    }

    private void spawnAtStart()
    {
        defaultPoint = GameObject.Find("DefaultSpawnPoint").transform;
        Instantiate(Player, defaultPoint.position, defaultPoint.rotation);
    }

    private void ResetLocation()
    {
        spawnLocation = "DefaultSpawnPoint";
        setPoint = false;
    }


}
