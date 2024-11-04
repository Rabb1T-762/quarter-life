using _Project.Scripts;

public sealed class KeyInteractionGreen : KeyInteraction
{
    public override void Interact(PlayerCharacterController playerCharacterController)
    {
        playerCharacterController.PickupGreenKey();
        base.Interact(playerCharacterController);
    }
}
