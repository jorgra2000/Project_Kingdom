using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    [Header("Tower")]
    [SerializeField] private LayerMask layerMaskAttack;
    [SerializeField] private Vector3 attackRangeDimensions;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Bullet projectilePrefab;

    private Transform targetToAttack;
    private List<GameObject> detectedObjects;

    void Start()
    {
        base.Start();
        StartCoroutine(AttackProcess());
        detectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if(CanBeInteracted && IsBuilt)
            targetToAttack = DetectNearTarget();
    }

    private Transform DetectNearTarget()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, attackRangeDimensions, Quaternion.identity, layerMaskAttack);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                detectedObjects.Add(col.gameObject);
            }
        }

        if (detectedObjects.Count != 0)
        {
            return detectedObjects[0].transform;
        }

        return null;
    }

    IEnumerator AttackProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (CanBeInteracted && IsBuilt) 
            {
                Bullet bullet = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
                bullet.SetTarget(targetToAttack);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackRangeDimensions * 2);
    }
}
