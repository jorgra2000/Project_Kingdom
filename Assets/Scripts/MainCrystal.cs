using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCrystal : Interactable
{
    [SerializeField] private float maxLightLevel;
    [SerializeField] private float maxSafeZoneRadius;
    [SerializeField] private LayerMask affectLightLayer;
    [SerializeField] private GameManager gameManager;

    private float currentLightLevel;
    private float currentSafeZoneRadius;

    private List<Building> allBuildings = new List<Building>();

    public float CurrentSafeZoneRadius { get => currentSafeZoneRadius; set => currentSafeZoneRadius = value; }

    private void Start()
    {
        currentLightLevel = 10;
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
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLightLevel = 2;
            currentSafeZoneRadius = 2;
            UpdateSafeZone();
        }
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

            /*var zoneAffectable = col.GetComponent<IZoneAffectable>();
            if (zoneAffectable != null)
            {
                zoneAffectable.OnZoneUpdate(inside);
            }*/
        }
        CheckVictory();
    }

    public void ChangeLight(float light) 
    {
        currentLightLevel += light;
        UpdateSafeZone();
    }

    public override void Interact()
    {
        currentLightLevel += 40;
        currentSafeZoneRadius += 40;
        UpdateSafeZone();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentSafeZoneRadius);
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
