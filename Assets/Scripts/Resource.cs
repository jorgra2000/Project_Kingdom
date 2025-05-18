using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform target;
    private bool isAttracting = false;

    void Update()
    {
        if (isAttracting && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.3f)
            {
                Collect();
            }
        }
    }

    public void AttractTo(Transform player)
    {
        target = player;
        isAttracting = true;
    }

    private void Collect() 
    {
        target.GetComponent<PlayerLightSystem>().AddLight();
        Destroy(gameObject);
    }
}
