using _Project.Scripts;
using UnityEngine;

public sealed class CaseInteraction : MonoBehaviour, IInteractionObject
{
    [SerializeField] private string movingPartName = "Case Glass";
    private bool _isOpen = false;
    public void Interact(PlayerCharacterController playerController)
    {
        if(!_isOpen)
        {
            foreach(Transform child in transform)
            {
                if(child.name == movingPartName)
                {
                    _isOpen = true;
                    Destroy(child.gameObject);
                }
            }
        }          
    }

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController)
    {
    }

    public void DisableInteractionUI(PlayerCharacterController playerCharacterController)
    {
    }

    public bool IsWithinInteractionArea()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 5f);

        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent<PlayerCharacterController>(out PlayerCharacterController player))
            {
                return true;
            }
        }
        return false;
    }
}
