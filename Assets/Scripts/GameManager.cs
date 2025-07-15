using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UISystem uISystem;
    [SerializeField] PlayerInput playerInput;

    [Header ("Day/Night Cicle")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0, 24)] private float timeDay;
    [SerializeField] private float dayDurationInSeconds = 420f;

    private float timeScale;
    private bool isNight;

    public float TimeDay { get => timeDay; set => timeDay = value; }
    public float TimePercent => timeDay / 24f;

    public bool IsNight { get => isNight; set => isNight = value; }

    void Start()
    {
        timeScale = 24f / dayDurationInSeconds;
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            timeDay += Time.deltaTime * timeScale;
            if (timeDay >= 24f)
            {
                timeDay %= 24f;
            }
            UpdateLighting(timeDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if (directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }


    private void OnValidate()
    {
        if (directionalLight != null)
        {
            return;
        }
        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }

    public void ChangeDay(float timeRespawn, float decrementTimeRespawn) 
    {
        timeRespawn -= decrementTimeRespawn;
    }

    public void PauseGame() 
    {
        Time.timeScale = 0f;
        uISystem.ShowPauseMenu();
        playerInput.SwitchCurrentActionMap("UI");
    }

    public void ResumeGame() 
    {
        Time.timeScale = 1f;
        if (uISystem == null)
        {
            uISystem = FindObjectOfType<UISystem>();
        }
        uISystem.QuitPauseMenu();
        playerInput.SwitchCurrentActionMap("Main");
    }

    public void Victory() 
    {
        uISystem.ShowVictoryMenu();
        StartCoroutine(GoBackToSelectLevel());
    }

    public void Defeat() 
    {
        uISystem.ShowDefeatMenu();
        StartCoroutine(GoBackToSelectLevel());
    }

    IEnumerator GoBackToSelectLevel() 
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene(1);
    }
}
