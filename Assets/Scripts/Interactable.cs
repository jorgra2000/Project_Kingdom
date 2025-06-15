using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour, IInteractable
{
    [Header ("Interaction")]
    [SerializeField] private Image interactImage;
    [SerializeField] private Image filledInteract;
    [SerializeField] private float timeToInteract;
    [SerializeField] private SphereCollider interactionTrigger;
    [SerializeField] private int lightCost;
    private bool canBeInteracted = true;
    private bool isInteracting = false;

    public int LightCost { get => lightCost; set => lightCost = value; }

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
        interactImage.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        interactImage.gameObject.SetActive(true);
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
