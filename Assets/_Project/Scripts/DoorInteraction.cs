using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractionObject
{
    private bool isOn = true;
    [SerializeField] private Material Green;
    [SerializeField] private Material Red;
    public void Interact(PlayerCharacterController playerController)
    {
        Debug.Log("Change box to green");
        isOn = !isOn;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
            {
                if (mesh)
                {
                    if (isOn)
                    {
                        mesh.material = Green;
                    }
                    else
                    {
                        mesh.material = Red;
                    }
                }
            }
            if (child.TryGetComponent<DoorInteraction>(out DoorInteraction doorInteraction))
            {
                if (doorInteraction)
                {
                    if (doorInteraction.transform.TryGetComponent(out MeshRenderer doorMesh))
                    {
                        if (isOn)
                        {
                            doorMesh.material = Green;
                        }
                        else
                        {
                            doorMesh.material = Red;
                        }
                    }

                }
            }
        }

    }

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController)
    {
        Debug.Log("E");
    }

    public bool IsWithinInteractionArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<PlayerCharacterController>(out PlayerCharacterController player))
            {
                return true;
            }
        }
        return false;
    }


}
