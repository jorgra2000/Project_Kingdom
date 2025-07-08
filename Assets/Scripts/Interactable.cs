using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour, IInteractable
{
    [Header ("Interaction")]
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private Image filledInteract;
    [SerializeField] private float timeToInteract;
    [SerializeField] private SphereCollider interactionTrigger;
    [SerializeField] private int lightCost;
    private bool canBeInteracted = true;
    private bool isInteracting = false;

    public int LightCost { get => lightCost; set => lightCost = value; }
    public bool CanBeInteracted { get => canBeInteracted; set => canBeInteracted = value; }

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isInteracting) 
        {
           filledInteract.fillAmount += Time.deltaTime / timeToInteract;
            if(filledInteract.fillAmount >= 1) 
            {
                StopInteract();
            }
        }
    }

    public virtual void Interact()
    {
    }

    public void StartInteraction() 
    {
        isInteracting = true;
    }

    public void StopInteract() 
    {
        isInteracting = false;
        filledInteract.fillAmount = 0;
    }

    public void HideText() 
    {
        interactPanel.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        if(canBeInteracted)
            interactPanel.gameObject.SetActive(true);
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
