using _Project.Scripts;
using UnityEngine;

public sealed class DoorInteractionBlue : DoorInteraction
{
    public override void Interact(PlayerCharacterController playerController)
    {
        if(playerController.HasBlueKey())
        {
            base.Interact(playerController);    
        }
    }

    public override void DisplayInteractionUI(PlayerCharacterController playerController)
    {
        if(playerController.HasBlueKey())
        {
            base.DisplayInteractionUI(playerController);
        } 
        else
        {
            Debug.Log("You require a Blue keycard.");
        }
    }
}
