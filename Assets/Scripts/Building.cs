using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable
{
    [SerializeField] private int lightCost;
    [SerializeField] private Mesh builtMesh;

    private bool isBuilt = false;
    private bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Interact()
    {
        BuildStructure();
    }

    private void BuildStructure() 
    {
        GetComponent<MeshFilter>().mesh = builtMesh;
        isBuilt = true;
    }
}
