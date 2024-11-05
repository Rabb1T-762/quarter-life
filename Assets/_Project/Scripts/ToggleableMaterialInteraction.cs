using _Project.Scripts;
using UnityEngine;

public class ToggleableMaterialInteraction : MonoBehaviour, IInteractionObject
{
    private bool isOn = true;
    [SerializeField] private Material Off;
    [SerializeField] private Material On;
    [SerializeField] private Transform changingPart;
    public void Interact(PlayerCharacterController playerController)
    {
        isOn = !isOn;

        if(changingPart.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
        {
            // NOTE: Removed if(mesh)
            if (isOn)
            {
                Material[] materials = changingPart.GetComponent<Renderer>().sharedMaterials;
                int alteredMaterialIndex = System.Array.IndexOf(materials, On);
                materials[alteredMaterialIndex] = Off;
                changingPart.GetComponent<Renderer>().materials = materials;
            }
            else
            {
                Material[] materials = changingPart.GetComponent<Renderer>().sharedMaterials;
                int alteredMaterialIndex = System.Array.IndexOf(materials, Off);
                materials[alteredMaterialIndex] = On;
                changingPart.GetComponent<Renderer>().materials = materials;
            }
        }

    }

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController)
    {
    }

    public virtual void DisableInteractionUI(PlayerCharacterController playerCharacterController)
    {    
    }

    public bool IsWithinInteractionArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<PlayerCharacterController>(out PlayerCharacterController player))
            {
                return true;
            }
        }
        return false;
    }


}
