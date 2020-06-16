using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMenu
{
    public class MenuOption
    {
        public Action<Interaction> action { get; private set; }
        public string name { get; private set; }

        public MenuOption(string name, Action<Interaction> action)
        {
            this.name = name;
            this.action = action;
        }
    }

    private Dictionary<InteractionType, List<MenuOption>> interactionOptions = new Dictionary<InteractionType, List<MenuOption>>();

    private List<Action<Interaction>> defaultInteractions = new List<Action<Interaction>>();

    public void Interact(Interaction interaction, Transform target)
    {
        if (!interactionOptions.ContainsKey(interaction.type) || interactionOptions[interaction.type] == null || interactionOptions[interaction.type].Count == 0)
        {
            RunAll(interaction, defaultInteractions);
            return;
        }

        DisplayAll(target, interaction, interactionOptions[interaction.type]);
    }

    public void AddMenuOption(InteractionType type, MenuOption option)
    {
        if (!interactionOptions.ContainsKey(type))
            interactionOptions.Add(type, new List<MenuOption>());

        interactionOptions[type].Add(option);
    }

    public void AddDefaultInteraction(Action<Interaction> action)
    {
        defaultInteractions.Add(action);
    }

    private void DisplayAll(Transform target, Interaction interaction, List<MenuOption> options)
    {
        InteractionMenuController.ShowInteractionMenu(target, options, option => option.action(interaction));
    }

    private void RunAll(Interaction interaction, List<Action<Interaction>> actions)
    {
        foreach (Action<Interaction> action in actions)
            action(interaction);
    }
}
