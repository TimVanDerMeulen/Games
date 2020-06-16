using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : InteractableController
{
    public bool state;

    public new void Awake()
    {
        base.Awake();

        interactionMenu.AddMenuOption(InteractionType.USE, new InteractableMenu.MenuOption("Flip", interaction => { state = !state; }));
    }

}
