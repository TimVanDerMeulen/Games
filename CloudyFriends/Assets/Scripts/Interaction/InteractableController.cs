using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour, Interactable
{
    protected InteractableMenu interactionMenu;

    public void Awake() {
        interactionMenu = new InteractableMenu();
    }

    public void Interact(Interaction interaction){
        interactionMenu.Interact(interaction, transform);
    }
}
