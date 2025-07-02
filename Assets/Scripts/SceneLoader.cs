using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;

    public void ChangeScene(int index) 
    {
        SceneManager.LoadScene(index);
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void OpenOptions() 
    {
        optionsPanel.SetActive(true);
    }

    public void ApplyOptions()
    {
        optionsPanel.SetActive(false);
    }
}
