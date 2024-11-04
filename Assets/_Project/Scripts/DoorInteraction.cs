using System.Collections;
using _Project.Scripts;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractionObject
{
    [SerializeField] private string movingPartName = "Unlocked Door Slider";
    [SerializeField] private float closeDelay = 5f;
    private bool _isOpen = false;
    public virtual void Interact(PlayerCharacterController playerController)
    {
        if(!_isOpen)
        {
            if(this.transform.name == movingPartName)
            {
                StartCoroutine(MoveDoor(this.transform, new Vector3(2.5f,0,0)));
            } 
        }     
    }

    private IEnumerator MoveDoor(Transform transform, Vector3 moveDir)
    {
        transform.Translate(moveDir);
        _isOpen = true;
        yield return new WaitForSeconds(closeDelay);
        transform.Translate(-moveDir);
        _isOpen = false;
    }

    public virtual void DisplayInteractionUI(PlayerCharacterController playerCharacterController)
    {
        Debug.Log("E to open door.");
    }

    public virtual void DisableInteractionUI(PlayerCharacterController playerCharacterController)
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
