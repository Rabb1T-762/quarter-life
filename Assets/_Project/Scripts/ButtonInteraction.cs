using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour, IInteractionObject
{
    [SerializeField] Material doorButtonOpened;
    public void Interact(PlayerCharacterController playerController)
    {
        bool hasKey = playerController.HasKey();

        if(hasKey)
        {
            //do some cool shit here
            foreach(Transform child in transform)
            {
                if(child.TryGetComponent<MeshRenderer>(out MeshRenderer mesh) && child.name == "ButtonVisual")
                {
                    mesh.material = doorButtonOpened;
                }
                if(child.name == "MainDoor")
                {
                    foreach(Transform doorSide in child)
                    {
                        if(doorSide.name == "Door_Big_1")
                        {
                            doorSide.Translate(new Vector3(-2.5f, 0, 0));
                        }
                        if(doorSide.name == "Door_Big_2")
                        {
                            doorSide.Translate(new Vector3(2.5f, 0, 0));
                        }
                    }
                }
            }
            Debug.Log("DOOR OPENS WEEWOO");
        }
        else
        {
            Debug.Log("Access Denied Loser");
        }
    }

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController)
    {
        bool hasKey = playerCharacterController.HasKey();

        if(hasKey)
        {
            Debug.Log("E");
        }
        else
        {
            Debug.Log("X");
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
