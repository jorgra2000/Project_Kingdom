using System.Collections;
using UnityEngine;

public class LightGenerator : MonoBehaviour, IInteractable
{

    [SerializeField] private float timeToReload;
    [SerializeField] private int lightAmount;
    [SerializeField] private Resource resourcePrefab;

    

    private bool readyToLight = true;

    void Start()
    {
        StartCoroutine(RecoverLight());
        var renderer = GetComponent<Renderer>();
    }

    public void Interact()
    {
        if (readyToLight) 
        {
            readyToLight = false;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            StartCoroutine(SpawnWithDelay());
        }
    }

    private IEnumerator SpawnWithDelay()
    {
        for (int i = 0; i < lightAmount; i++)
        {
            Instantiate(resourcePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RecoverLight() 
    {
        while (true) 
        {
            if (!readyToLight) 
            {
                yield return new WaitForSeconds(timeToReload);
                GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
                readyToLight = true;
            }
            yield return null;
        }
    }

}
