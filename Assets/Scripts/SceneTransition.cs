using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName; // Nombre de la escena a cargar
    [SerializeField] private string spawnLocation;

    private void Start() {
    }

    public void onSceneChange()
    {
        SpawnManager.Instance.SetSpwanLocation(spawnLocation);
        LevelLoader.LoadLevel(sceneName);
    }
}
