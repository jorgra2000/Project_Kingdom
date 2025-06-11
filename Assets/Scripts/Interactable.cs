using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [Header ("Interaction")]
    [SerializeField] private TextMeshPro interactText;
    [SerializeField] private float timeToInteract;
    [SerializeField] private SphereCollider interactionTrigger;
    private bool canBeInteracted;
    

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interact()
    {
    }

    public void HideText() 
    {
        interactText.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        interactText.gameObject.SetActive(true);
    }

    public bool GetCanInteract() 
    {
        return canBeInteracted;
    }

    public void SetCanInteract(bool value)
    {
        canBeInteracted = value;
    }

    public void ActivateInteractionTrigger() 
    {
        interactionTrigger.enabled = true;
    }

    public void DesactivateInteractionTrigger()
    {
        interactionTrigger.enabled = false;
    }
}
