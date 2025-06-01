using TMPro;
using UnityEngine;

public class PlayerLightSystem : MonoBehaviour
{
    [SerializeField] private LayerMask lightMask;
    [SerializeField] private int maxLight;
    [SerializeField] private float attractionRadius;
    [SerializeField] private TextMeshProUGUI lightText;
    [SerializeField] private Light staffLight;
    private int currentLight = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLight();
    }

    // Update is called once per frame
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
        lightText.text = currentLight + " / " + maxLight;
        staffLight.intensity = currentLight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
