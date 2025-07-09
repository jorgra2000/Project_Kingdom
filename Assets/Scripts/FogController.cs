using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    private VisualEffect fogEffect;

    [SerializeField] private MainCrystal mainCrystal;
    [SerializeField] private GameManager gameManager;

    private float timePercent;
    private float radius;
    private float alpha;

    // Start is called before the first frame update
    void Start()
    {
        fogEffect = GetComponent<VisualEffect>();

        fogEffect.SetVector3("ColliderCenter", mainCrystal.gameObject.transform.position);
        fogEffect.SetFloat("ColliderRadius", mainCrystal.CurrentSafeZoneRadius);
    }

    // Update is called once per frame
    void Update()
    {
        timePercent = gameManager.TimePercent;

        // 5:00 a 6:00 → transición de mínimo a máximo
        if (timePercent >= 0.208f && timePercent < 0.25f)
        {
            float t = Mathf.InverseLerp(0.208f, 0.25f, timePercent);
            radius = Mathf.Lerp(mainCrystal.CurrentSafeZoneRadius, mainCrystal.MaxSafeZoneRadius, t);
            alpha = Mathf.Lerp(1f, 0f, t);
        }
        // 22:00 a 23:00 → transición de máximo a mínimo
        else if (timePercent >= 0.916f && timePercent < 0.958f)
        {
            float t = Mathf.InverseLerp(0.916f, 0.958f, timePercent);
            radius = Mathf.Lerp(mainCrystal.MaxSafeZoneRadius, mainCrystal.CurrentSafeZoneRadius, t);
            alpha = Mathf.Lerp(0f, 1f, t);
        }
        // 23:00 a 5:00 → niebla cerca y opaca
        else if (timePercent >= 0.958f || timePercent < 0.208f)
        {
            radius = mainCrystal.CurrentSafeZoneRadius;
            alpha = 1f;
        }
        // 6:00 a 22:00 → niebla lejos y transparente
        else
        {
            radius = mainCrystal.MaxSafeZoneRadius;
            alpha = 0f;
        }

        fogEffect.SetVector3("ColliderCenter", mainCrystal.transform.position);
        fogEffect.SetFloat("ColliderRadius", radius);
        //fogEffect.SetFloat("Alpha", alpha); // Esto debe existir en tu VFX Graph
    }

    // Calcula qué tan "de día" es: 0 = noche, 1 = mediodía
    private float DaylightFactor(float timePercent)
    {
        return Mathf.Sin(timePercent * Mathf.PI);
    }
}
