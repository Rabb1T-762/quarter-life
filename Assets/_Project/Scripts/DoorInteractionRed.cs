using _Project.Scripts;
using UnityEngine;

public sealed class DoorInteractionRed : DoorInteraction
{
    public override void Interact(PlayerCharacterController playerController)
    {
        if(playerController.HasRedKey())
        {
            base.Interact(playerController);    
        }
    }

    public override void DisplayInteractionUI(PlayerCharacterController playerController)
    {
        if(playerController.HasRedKey())
        {
            base.DisplayInteractionUI(playerController);
        } 
        else
        {
            Debug.Log("You require a Red keycard");
        }
    }
}
