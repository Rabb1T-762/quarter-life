using _Project.Scripts;
using UnityEngine;

public class KeyInteractionRed : KeyInteraction
{
    public override void Interact(PlayerCharacterController playerCharacterController)
    {
        playerCharacterController.PickupRedKey();
        Destroy(gameObject);
    }
}
