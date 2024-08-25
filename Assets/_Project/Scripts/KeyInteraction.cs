using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

public class KeyInteraction : MonoBehaviour, IInteractionObject
{
    public void Interact(PlayerCharacterController playerCharacterController)
    {
        playerCharacterController.PickupKey();
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
