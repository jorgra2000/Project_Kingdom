using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    [SerializeField] private int lightCost;
    [SerializeField] private Mesh builtMesh;

    private bool isBuilt = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Interact()
    {
        if (!isBuilt)
        {
            BuildStructure();
        }
        else 
        {
            Debug.Log("Construido");
        }
    }

    private void BuildStructure() 
    {
        GetComponent<MeshFilter>().mesh = builtMesh;
        isBuilt = true;
    }
}
