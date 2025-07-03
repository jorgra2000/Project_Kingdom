using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject defeatMenu;
    [SerializeField] private GameObject victoryMenu;

    public void ShowPauseMenu() 
    {
        pauseMenu.SetActive(true);
    }

    public void QuitPauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void ShowDefeatMenu() 
    {
        defeatMenu.SetActive(true);
    }

    public void ShowVictoryMenu()
    {
        victoryMenu.SetActive(true);
    }
}
