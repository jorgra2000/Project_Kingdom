using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCrystal : Interactable
{
    [SerializeField] private float maxLightLevel;
    [SerializeField] private float startLightLevel;
    [SerializeField] private float maxSafeZoneRadius;
    [SerializeField] private float addLightFactor;
    [SerializeField] private LayerMask affectLightLayer;
    [Header("Scripts")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UISystem uISystem;
    [SerializeField] private PlayerLightSystem playerLightSystem;
    [Header("Mesh")]
    [SerializeField] private Transform meshCrystal;
    [SerializeField] private float rotationSpeed;
    

    private float currentLightLevel;
    private float currentSafeZoneRadius;

    private List<Building> allBuildings = new List<Building>();

    public float CurrentSafeZoneRadius { get => currentSafeZoneRadius; set => currentSafeZoneRadius = value; }
    public float MaxLightLevel { get => maxLightLevel; set => maxLightLevel = value; }
    public float MaxSafeZoneRadius { get => maxSafeZoneRadius; set => maxSafeZoneRadius = value; }

    private void Start()
    {
        currentLightLevel = startLightLevel;
        currentSafeZoneRadius = (currentLightLevel / maxLightLevel) * maxSafeZoneRadius;

        Collider[] colliders = Physics.OverlapSphere(transform.position, maxSafeZoneRadius, affectLightLayer);
        foreach (Collider col in colliders)
        {
            Building building = col.GetComponent<Building>();
            if (building != null && !allBuildings.Contains(building))
            {
                allBuildings.Add(building);
            }
        }

        UpdateSafeZone();
        uISystem.UpdateSlider(currentLightLevel, maxLightLevel);
    }


    public override void Update()
    {
        base.Update();
        meshCrystal.Rotate(new Vector3(0,0,rotationSpeed) * Time.deltaTime);
    }

    public void UpdateSafeZone()
    {
        currentSafeZoneRadius = (currentLightLevel / maxLightLevel) * maxSafeZoneRadius;

        foreach (Building building in allBuildings)
        {
            float dist = Vector3.Distance(transform.position, building.transform.position);
            bool inside = dist <= currentSafeZoneRadius;

            Building colBuilding = building.GetComponent<Building>();

            if (inside)
            {
                colBuilding.SetCanInteract(true);
            }
            else
            {
                colBuilding.SetCanInteract(false);
            }
        }
        CheckVictory();
    }

    public void ChangeLight(float light) 
    {
        currentLightLevel += light;
        uISystem.UpdateSlider(currentLightLevel, maxLightLevel);
        UpdateSafeZone();
    }

    public override void Interact()
    {
        ChangeLight(addLightFactor);
        UpdateSafeZone();
        playerLightSystem.LoseLight(LightCost);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentSafeZoneRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxSafeZoneRadius);
    }

    void CheckVictory() 
    {
        if(currentLightLevel >= maxLightLevel) 
        {
            gameManager.Victory();
        }
        else if(currentLightLevel <= 0) 
        {
            gameManager.Defeat();
        }
    }
}
