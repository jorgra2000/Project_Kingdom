using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    private VisualEffect fogEffect;

    [SerializeField] private MainCrystal mainCrystal;

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
        fogEffect.SetFloat("ColliderRadius", mainCrystal.CurrentSafeZoneRadius);
    }
}
