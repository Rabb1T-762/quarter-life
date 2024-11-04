using _Project.Scripts;

public interface IInteractionObject
{
    public void Interact(PlayerCharacterController playerController);

    // Method that fires when you look at the interactable
    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController);

    // Method that fires when you look away from the interactable
    public void DisableInteractionUI(PlayerCharacterController playerCharacterController);

    public bool IsWithinInteractionArea();
}
