using _Project.Scripts;

public sealed class KeyInteractionBlue : KeyInteraction
{
    public override void Interact(PlayerCharacterController playerCharacterController)
    {
        playerCharacterController.PickupBlueKey();
        base.Interact(playerCharacterController);
    }
}
