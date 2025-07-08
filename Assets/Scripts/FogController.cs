using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    private VisualEffect fogEffect;

    [SerializeField] private MainCrystal mainCrystal;
    [SerializeField] private GameManager gameManager;

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
        float t = DaylightFactor(gameManager.TimePercent); // entre 0 (noche) y 1 (día)

        float radius = Mathf.Lerp(mainCrystal.CurrentSafeZoneRadius, mainCrystal.MaxSafeZoneRadius, t);
        fogEffect.SetFloat("ColliderRadius", radius);
    }

    // Calcula qué tan "de día" es: 0 = noche, 1 = mediodía
    private float DaylightFactor(float timePercent)
    {
        return Mathf.Sin(timePercent * Mathf.PI);
    }
}
