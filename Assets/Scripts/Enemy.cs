using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float maxHealth;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (currentObjective == null)
        {
            currentObjective = crystalPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal")) 
        {
            crystalScript.ChangeLight(-5f);
            Destroy(this.gameObject);
        }
    }
}
