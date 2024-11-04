using _Project.Scripts;
using UnityEngine;

public class KeyInteraction : MonoBehaviour, IInteractionObject
{
    protected bool _hasBeenPickedUp = false;
    public virtual void Interact(PlayerCharacterController playerCharacterController)
    {
        if(!_hasBeenPickedUp)
        {
            Destroy(gameObject);
            _hasBeenPickedUp = true;
        }
    }

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController)
    {
        // Outline script should already be on the object, so set the outline width
        // instead of creating and destroying an Outline every time (how it was done before)
        if(gameObject.TryGetComponent<Outline>(out Outline outline))
        {
            outline.OutlineWidth = 5f;
        }
    }

    public void DisableInteractionUI(PlayerCharacterController playerCharacterController)
    {
        // Got some fun null reference exceptions when not doing this check.
        if(!_hasBeenPickedUp)
        {
            Outline outline = gameObject.GetComponent<Outline>();
            outline.OutlineWidth = 0f;
        }
    }

    public bool IsWithinInteractionArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent<PlayerCharacterController>(out PlayerCharacterController player))
            {
                return true;
            }
        }
        return false;
    }
}
