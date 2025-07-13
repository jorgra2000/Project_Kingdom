using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    private VisualEffect fogEffect;

    [SerializeField] private MainCrystal mainCrystal;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController player;
    [SerializeField] private float damagePerSecond;

    private float timePercent;
    private float radius;
    private float alpha;
    private float distanceToPlayer;

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
        distanceToPlayer = Vector3.Distance(player.transform.position, mainCrystal.transform.position);
        timePercent = gameManager.TimePercent;

        // 5:00 a 6:00 → Terminan las oleadas
        if (timePercent >= 0.208f && timePercent < 0.25f)
        {
            float t = Mathf.InverseLerp(0.208f, 0.25f, timePercent);
            radius = Mathf.Lerp(mainCrystal.CurrentSafeZoneRadius, mainCrystal.MaxSafeZoneRadius, t);
            gameManager.IsNight = false;
        }
        // 22:00 a 23:00 → transición de máximo a mínimo
        else if (timePercent >= 0.916f && timePercent < 0.958f)
        {
            float t = Mathf.InverseLerp(0.916f, 0.958f, timePercent);
            radius = Mathf.Lerp(mainCrystal.MaxSafeZoneRadius, mainCrystal.CurrentSafeZoneRadius, t);
        }
        // 23:00 a 5:00 → Empiezan las oleadas
        else if (timePercent >= 0.958f || timePercent < 0.208f)
        {
            radius = mainCrystal.CurrentSafeZoneRadius;
            gameManager.IsNight = true;
            CheckDamagePlayer();
        }
        // 6:00 a 22:00 → niebla lejos y transparente
        else
        {
            radius = mainCrystal.MaxSafeZoneRadius;
        }

        fogEffect.SetVector3("ColliderCenter", mainCrystal.transform.position);
        fogEffect.SetFloat("ColliderRadius", radius);
    }

    void CheckDamagePlayer() 
    {
        if (gameManager.IsNight) 
        {
            if (distanceToPlayer > radius)
            {
                player.TakeDamageOverTime(damagePerSecond * Time.deltaTime);
            }
            else 
            {
                player.Health();
            }
        }
        else 
        {
            player.Health();
        }
    }
}
