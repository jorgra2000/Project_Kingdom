using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    [SerializeField] private Mesh builtMesh;

    private bool isBuilt = false;


    // Start is called before the first frame update
    void Start()
    {
        SetCanInteract(false);
    }

    public override void Interact()
    {
        if (!isBuilt)
        {
            BuildStructure();
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
        GetComponent<MeshFilter>().mesh = builtMesh;
        isBuilt = true;
    }
}
