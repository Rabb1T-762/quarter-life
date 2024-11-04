using System.Collections;
using _Project.Scripts;
using UnityEngine;

public sealed class ToggleableMovementInteraction : MonoBehaviour, IInteractionObject
{
    [SerializeField] private string movingPartName = "Lid";

    [SerializeField] private Vector3 rotateDir = new Vector3(-90f, 0, 0);

    [SerializeField] private Vector3 relativeDisplacedPosition = Vector3.zero;

    [SerializeField] private Vector3 relativeOriginalPosition = Vector3.zero;
    private bool _isDisplaced = false;
    public void Interact(PlayerCharacterController playerController)
    {
        Transform movingPart = (this.transform.name == movingPartName) ? this.transform : null;

        if(!movingPart)
        {
            foreach(Transform child in this.transform)
            {
                if(child.name == movingPartName)
                {
                    movingPart = child;
                }
            }
        }

        if(!_isDisplaced)
        {
            MovePart(movingPart, relativeDisplacedPosition);
            RotatePart(movingPart, rotateDir);
        } else
        {
            MovePart(movingPart, relativeOriginalPosition);
            RotatePart(movingPart, -rotateDir);
        }  

        _isDisplaced = !_isDisplaced;             
    }         

    private void RotatePart(Transform transform, Vector3 rotateDir)
    {
        transform.Rotate(rotateDir);
    }

    private void MovePart(Transform transform, Vector3 moveDestination)
    {
        if(moveDestination != Vector3.zero)
        {
            // NOTE: Think about ways to inject this behaviour so that you can choose to do other things
            // like Translate instead
            transform.localPosition = moveDestination;
        }    
    }

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController)
    {
        // NOTE: Not all ToggleableMovement instances want nothing to happen, so might just inherit from this
    }

    public void DisableInteractionUI(PlayerCharacterController playerCharacterController)
    {
        // NOTE: Not all ToggleableMovement instances want nothing to happen, so might just inherit from this
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
