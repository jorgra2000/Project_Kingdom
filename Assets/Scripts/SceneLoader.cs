using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) 
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

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

    public void SetVolume(float volume) 
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int indexQuality) 
    {
        QualitySettings.SetQualityLevel(indexQuality);
    }

    public void SetFullScreen(bool isFullScreen) 
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex) 
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
