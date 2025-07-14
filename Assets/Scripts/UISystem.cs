using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject defeatMenu;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private Slider crystalSlider;
    [SerializeField] private Image healthbar;
    [SerializeField] private Image lightStaff;
    [SerializeField] private TextMeshProUGUI lightText;

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

    public void UpdateHealthBar(float health, float maxHealth)
    {
        healthbar.fillAmount = health/maxHealth;
    }
    public void UpdateLight(float light, float maxLight)
    {
        lightStaff.fillAmount = light / maxLight;
        lightText.text = light + " / " + maxLight;
    }
}
