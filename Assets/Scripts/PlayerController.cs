using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

public class PlayerController : MonoBehaviour
{
    private const string IDLE = "Idle";
    private const string RUN = "Run";

    private PlayerControls playerControls;

    private NavMeshAgent agent;
    private Animator animator;

    private GameObject buildingToInteract;

    [Header("Movement")]
    [SerializeField] private ParticleSystem clickEffectPrefab;
    [SerializeField] private LayerMask clickableLayers;

    private ParticleSystem clickEffectInstance;

    private void Awake()
    {
        playerControls = new PlayerControls();
        AssignInputs();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (clickEffectPrefab != null)
        {
            clickEffectInstance = Instantiate(clickEffectPrefab);
            clickEffectInstance.Stop();
        }
    }

    private void Update()
    {
        SetAnimation();
    }

    private void AssignInputs()
    {
        playerControls.Main.Move.performed += ctx => ClickToMove();
        playerControls.Main.Interact.performed += ctx => Interact();
    }

    private void ClickToMove() 
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers)) 
        {
            agent.destination = hit.point;

            if(clickEffectInstance != null) 
            {
                clickEffectInstance.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                clickEffectInstance.transform.rotation = Quaternion.identity;
                clickEffectInstance.Stop();
                clickEffectInstance.Play();
            }
        }
    }

    void SetAnimation() 
    {
        if(agent.velocity == Vector3.zero) 
        {
            animator.Play(IDLE);
        }
        else 
        {
            animator.Play(RUN);
        }
    }

    void Interact() 
    {
        if(buildingToInteract != null) 
        {
            buildingToInteract.GetComponent<IInteractable>().Interact();
        }
    }

    private void OnEnable(){ playerControls.Enable(); }

    private void OnDisable(){ playerControls.Disable(); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Building")) 
        {
            buildingToInteract = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Building"))
        {
            buildingToInteract = null;
        }
    }


}
