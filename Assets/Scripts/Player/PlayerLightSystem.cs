using TMPro;
using UnityEngine;

public class PlayerLightSystem : MonoBehaviour
{
    [SerializeField] private LayerMask lightMask;
    [SerializeField] private int maxLight;
    [SerializeField] private float attractionRadius;
    [SerializeField] private Light staffLight;
    [SerializeField] private UISystem uISystem;

    private int currentLight = 0;

    public int CurrentLight { get => currentLight; set => currentLight = value; }


    // Start is called before the first frame update
    void Start()
    {
        UpdateLight();
    }

    void Update()
    {
        Collider[] coins = Physics.OverlapSphere(transform.position, attractionRadius, lightMask);
        foreach (Collider coinCollider in coins) 
        {
            Resource coin = coinCollider.GetComponent<Resource>();
            if (coin != null)
            {
                coin.AttractTo(transform);
            }
        }
    }

    public void AddLight() 
    {
        currentLight++;
        UpdateLight();
    }

    public void LoseLight(int light) 
    {
        currentLight -= light;
        UpdateLight();
    }

    private void UpdateLight() 
    {
        staffLight.intensity = currentLight;
        uISystem.UpdateLight(currentLight, maxLight );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
