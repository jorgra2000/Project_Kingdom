using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject defeatMenu;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private Slider crystalSlider;

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

    public void UpdateSlider(float light, float maxLight) 
    {
        float normalized = crystalSlider.value = light/maxLight;
        crystalSlider.value = Mathf.Lerp(0.1f, 0.9f, normalized);
    }
}
