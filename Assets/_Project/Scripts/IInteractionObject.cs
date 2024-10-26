using _Project.Scripts;

public interface IInteractionObject
{
    public void Interact(PlayerCharacterController playerController);

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController);

    public bool IsWithinInteractionArea();
}
