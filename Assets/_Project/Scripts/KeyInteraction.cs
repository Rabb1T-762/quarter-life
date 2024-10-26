using _Project.Scripts;
using UnityEngine;
using System;

public class KeyInteraction : MonoBehaviour, IInteractionObject
{
    public virtual void Interact(PlayerCharacterController playerCharacterController)
    {
        throw new NotImplementedException("Key doesn't implement Interact");
    }

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController)
    {
        Debug.Log("E");
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
