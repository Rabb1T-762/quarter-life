using _Project.Scripts;
using UnityEngine;

public class KeyInteractionGreen : KeyInteraction
{
    public override void Interact(PlayerCharacterController playerCharacterController)
    {
        playerCharacterController.PickupGreenKey();
        Destroy(gameObject);
    }
}
