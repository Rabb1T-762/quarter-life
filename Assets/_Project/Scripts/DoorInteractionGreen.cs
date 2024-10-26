using System.Collections;
using _Project.Scripts;
using UnityEngine;

public sealed class DoorInteractionGreen : DoorInteraction
{
    public override void Interact(PlayerCharacterController playerController)
    {
        if(playerController.HasGreenKey())
        {
            base.Interact(playerController);   
        }
    }

    public override void DisplayInteractionUI(PlayerCharacterController playerController)
    {
        if(playerController.HasGreenKey())
        {
            base.DisplayInteractionUI(playerController);
        } 
        else
        {
            Debug.Log("You require a Green keycard");
        }
    }
}
