using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UISystem uISystem;
    [SerializeField] PlayerInput playerInput;

    [Header ("Day/Night Cicle")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0, 24)] private float timeDay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            timeDay += Time.deltaTime;
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

    public void PauseGame() 
    {
        Time.timeScale = 0f;
        uISystem.ShowPauseMenu();
        playerInput.SwitchCurrentActionMap("UI");
    }

    public void ResumeGame() 
    {
        Time.timeScale = 1f;
        uISystem.QuitPauseMenu();
        playerInput.SwitchCurrentActionMap("Main");
    }
}
