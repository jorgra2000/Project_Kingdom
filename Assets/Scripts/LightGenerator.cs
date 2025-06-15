using System.Collections;
using TMPro;
using UnityEngine;

public class LightGenerator : Interactable
{

    [SerializeField] private float timeToReload;
    [SerializeField] private int lightAmount;
    [SerializeField] private Resource resourcePrefab;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float launchForce;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float maxIntensity = 30f;

    private Material mat;
    private float minIntensity = 1f;



    private bool readyToLight = true;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        StartCoroutine(RecoverLight());
        var renderer = GetComponent<Renderer>();
        SetCanInteract(true);
    }

    public override void Update() 
    {
        base.Update();
        if (readyToLight)
        {
            float emission = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, emission);
            Color finalColor = Color.white * Mathf.LinearToGammaSpace(intensity);
            mat.SetColor("_EmissionColor", finalColor);
        }
    }

    public override void Interact()
    {
        if (readyToLight) 
        {
            readyToLight = false;
            mat.SetColor("_EmissionColor", Color.black);
            SetCanInteract(false);
            HideText();
            DesactivateInteractionTrigger();
            StartCoroutine(SpawnWithDelay());
        }
    }

    private IEnumerator SpawnWithDelay()
    {
        for (int i = 0; i < lightAmount; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = Mathf.Abs(randomOffset.y);

            Resource resource = Instantiate(resourcePrefab, spawnPoint.position, Quaternion.identity);

            Rigidbody rb = resource.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 launchDir = (randomOffset + Vector3.up * 0.5f).normalized;
                rb.AddForce(launchDir * launchForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator RecoverLight() 
    {
        while (true) 
        {
            if (!readyToLight) 
            {
                yield return new WaitForSeconds(timeToReload);
                SetCanInteract(true);
                mat.SetColor("_EmissionColor", Color.white);
                readyToLight = true;
                ActivateInteractionTrigger();
            }
            yield return null;
        }
    }

}
