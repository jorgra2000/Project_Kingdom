using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (readyToLight) 
        {
            readyToLight = false;
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
                readyToLight = true;
            }
            yield return null;
        }
    }

}
