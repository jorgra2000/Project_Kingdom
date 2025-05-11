using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

public class Player : MonoBehaviour
{
    private const string IDLE = "Idle";
    private const string RUN = "Run";

    private PlayerControls playerControls;

    private NavMeshAgent agent;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private LayerMask clickableLayers;

    private float lookRotationSpeed = 8f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        AssignInputs();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SetAnimation();
    }

    private void AssignInputs()
    {
        playerControls.Main.Move.performed += ctx => ClickToMove();
    }

    private void ClickToMove() 
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers)) 
        {
            agent.destination = hit.point;

            if(clickEffect != null) 
                Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
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

    private void OnEnable(){ playerControls.Enable(); }

    private void OnDisable(){ playerControls.Disable(); }
}
