using _Project.Scripts;

public sealed class KeyInteractionRed : KeyInteraction
{
    public override void Interact(PlayerCharacterController playerCharacterController)
    {
        playerCharacterController.PickupRedKey();
        Destroy(gameObject);
    }
}
