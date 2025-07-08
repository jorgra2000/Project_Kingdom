using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Building : Interactable
{
    [Header("Building")]
    [SerializeField] private Mesh builtMesh;
    [SerializeField] private ParticleSystem builtParticles;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private PlayerLightSystem playerLight;

    private bool isBuilt = false;

    public bool IsBuilt { get => isBuilt; set => isBuilt = value; }

    public virtual void Start()
    {
        costText.text = LightCost.ToString();
        SetCanInteract(false);
    }

    public override void Interact()
    {
        if (!isBuilt)
        {
            BuildStructure();
            playerLight.LoseLight(LightCost);
            LightCost = 0;
        }
        else 
        {
            Debug.Log("Construido");
        }
        StopInteract();
    }

    private void BuildStructure() 
    {
        builtParticles.Play();
        GetComponent<MeshFilter>().mesh = builtMesh;
        isBuilt = true;
        HideText();
        DesactivateInteractionTrigger();
    }
}
