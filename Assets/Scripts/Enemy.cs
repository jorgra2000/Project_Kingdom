using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float attack;
    [SerializeField] private float crystalDamage;
    [SerializeField] private Vector3 attackRangeDimensions;
    [Header("Effects")]
    [SerializeField] private GameObject deathParticles;

    private float currentHealth;
    private NavMeshAgent agent;
    private Transform crystalPosition;
    private Transform currentObjective;
    private MainCrystal crystalScript;
    private Animator animator;

    private Waypoint currentWaypoint;

    // Start is called before the first frame update

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        speed = agent.speed;

        crystalScript = FindAnyObjectByType<MainCrystal>();
        if (crystalScript != null)
        {
            crystalPosition = crystalScript.transform;
        }

        currentWaypoint = FindClosestWaypoint();

        if (currentWaypoint != null)
        {
            agent.SetDestination(currentWaypoint.transform.position);
        }
        else if (crystalPosition != null)
        {
            agent.SetDestination(crystalPosition.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento entre waypoints
        if (currentWaypoint != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypoint = currentWaypoint.NextWaypoint;

            if (currentWaypoint != null)
            {
                agent.SetDestination(currentWaypoint.transform.position);
            }
            else if (crystalPosition != null)
            {
                agent.SetDestination(crystalPosition.position);
            }
        }

        SetAnimation();
    }

    private Waypoint FindClosestWaypoint()
    {
        Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();
        Waypoint closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Waypoint wp in allWaypoints)
        {
            float distance = Vector3.Distance(transform.position, wp.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = wp;
            }
        }

        return closest;
    }

    void SetAnimation()
    {
        if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("moving", false);
        }
        else
        {
            animator.SetBool("moving", true);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death() 
    {
        Instantiate(deathParticles,transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal")) 
        {
            crystalScript.ChangeLight(-crystalDamage);
            Death();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackRangeDimensions * 2);
    }
}
