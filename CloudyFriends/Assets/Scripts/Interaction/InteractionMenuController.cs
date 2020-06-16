using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMenuController : MonoBehaviour
{
    private static InteractionMenuController INSTANCE;

    void Awake()
    {
        if (INSTANCE != null)
            throw new InvalidOperationException("InteractionMenuController may only be created once!");
        INSTANCE = this;
    }


    public static void ShowInteractionMenu(Transform target, List<InteractableMenu.MenuOption> options, Action<InteractableMenu.MenuOption> onSelection)
    {
        if (options.Count == 1)
        {
            onSelection(options[0]);
            return;
        }

        Debug.Log("Display Option Menu");
        //TODO do smth useful
    }
}
