using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEditor;
using UnityEngine;

public interface IInteractionObject
{
    public void Interact(PlayerCharacterController playerController);

    public void DisplayInteractionUI(PlayerCharacterController playerCharacterController);

    public bool IsWithinInteractionArea();
}
