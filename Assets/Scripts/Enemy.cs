using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    [SerializeField] private float attack;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Vector3 attackRangeDimensions;

    private float currentHealth;
    private NavMeshAgent agent;
    private Transform crystalPosition;
    private Transform currentObjective;
    private MainCrystal crystalScript;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        crystalScript = FindAnyObjectByType<MainCrystal>();
        crystalPosition = FindAnyObjectByType<MainCrystal>().gameObject.transform;
        currentObjective = crystalPosition;
        agent.destination = currentObjective.position;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentObjective == null)
        {
            currentObjective = crystalPosition;
        }

        SetAnimation();
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
            crystalScript.ChangeLight(-5f);
            Death();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackRangeDimensions * 2);
    }
}
