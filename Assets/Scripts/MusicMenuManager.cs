using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicMenuManager : MonoBehaviour
{
    private static MusicMenuManager instance;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else 
        {
            Destroy(this.gameObject);
        }

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex >= 2)
        {
            Destroy(this.gameObject);
        }
    }

}
