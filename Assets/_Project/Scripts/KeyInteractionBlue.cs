using _Project.Scripts;
using UnityEngine;

public class KeyInteractionBlue : KeyInteraction
{
    public override void Interact(PlayerCharacterController playerCharacterController)
    {
        playerCharacterController.PickupBlueKey();
        Destroy(gameObject);
    }
}
