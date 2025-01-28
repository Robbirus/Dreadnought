using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    public string promptMessage;

    // Add or Remove an InteractionEvent component to this gameObject
    public bool useEvents;

    public virtual string Onlook()
    {
        return promptMessage;
    }

    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().onInteract.Invoke();
        }
        Interact();
    }

    protected virtual void Interact()
    {

    }
 
}
