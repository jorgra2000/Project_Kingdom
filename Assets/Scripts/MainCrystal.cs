using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCrystal : MonoBehaviour, IInteractable
{
    [SerializeField] private float maxLightLevel;
    [SerializeField] private float maxSafeZoneRadius;
    [SerializeField] private LayerMask affectLightLayer;

    private float currentLightLevel;
    private float currentSafeZoneRadius;

    private void Start()
    {
        currentLightLevel = 10;
        currentSafeZoneRadius = 5;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateSafeZone()
    {
        currentSafeZoneRadius = (currentLightLevel / maxLightLevel) * maxSafeZoneRadius;

        //SHADERS
        // Actualizar efectos visuales como un shader de zona segura
        //Shader.SetGlobalFloat("_SafeZoneRadius", currentSafeZoneRadius);
        //Shader.SetGlobalVector("_SafeZoneCenter", transform.position);

        // Ejemplo de activación/desactivación de edificios
        Collider[] objects = Physics.OverlapSphere(transform.position, maxSafeZoneRadius, affectLightLayer);

        foreach (Collider col in objects)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            bool inside = dist <= currentSafeZoneRadius;

            Building colBuilding = col.GetComponent<Building>();

            if (!colBuilding.GetCanInteract()) 
            {
                colBuilding.SetCanInteract(true);
            }

            /*var zoneAffectable = col.GetComponent<IZoneAffectable>();
            if (zoneAffectable != null)
            {
                zoneAffectable.OnZoneUpdate(inside);
            }*/
        }
    }

    public void Interact()
    {
        currentLightLevel += 5;
        currentSafeZoneRadius += 5;
        UpdateSafeZone();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentSafeZoneRadius);
    }

}
