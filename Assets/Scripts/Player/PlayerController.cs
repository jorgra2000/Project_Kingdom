using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private NavMeshAgent agent;
    private Animator animator;
    private Interactable buildingToInteract;

    private float currentHealth;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerLightSystem lightSystem;

    [Header("Movement")]
    [SerializeField] private ParticleSystem clickEffectPrefab;
    [SerializeField] private LayerMask clickableLayers;

    [Header("Health")]
    [SerializeField] private float maxHealth;

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
        currentHealth = maxHealth;

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
        playerControls.Main.Interact.started += ctx => StartInteraction();
        playerControls.Main.Interact.performed += ctx => Interact();
        playerControls.Main.Interact.canceled += ctx => StopInteract();

        playerControls.Main.Pause.performed += ctx => gameManager.PauseGame();
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
            animator.SetBool("moving", false);
        }
        else 
        {
            animator.SetBool("moving", true);
        }
    }

    void Interact() 
    {
        if(buildingToInteract != null) 
        {
            if (lightSystem.CurrentLight >= buildingToInteract.LightCost)
            {
                buildingToInteract.Interact();
                lightSystem.CurrentLight -= buildingToInteract.LightCost;

            }
        }
    }

    void StartInteraction() 
    {
        if (buildingToInteract != null)
        {
            if (lightSystem.CurrentLight >= buildingToInteract.LightCost) 
            {
                buildingToInteract.StartInteraction();
            }
        }
    }

    void StopInteract() 
    {
        if(buildingToInteract != null)
            buildingToInteract.StopInteract();
    }

    void ChangeHealth(float lifePoints) 
    {
        currentHealth += lifePoints;
        if(currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }
    }

    private void OnEnable(){ playerControls.Enable(); }

    private void OnDisable(){ playerControls.Disable(); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactable>(out Interactable triggerObject)) 
        {
            if (triggerObject.GetCanInteract()) 
            {
                buildingToInteract = other.gameObject.GetComponent<Interactable>();
                triggerObject.ShowText();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactable>(out Interactable triggerObject))
        {
            StopInteract();
            buildingToInteract = null;
            triggerObject.HideText();
        }
    }
}
