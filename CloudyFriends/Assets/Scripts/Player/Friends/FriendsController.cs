using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FriendsController : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
    }

    public Settings settings;

    private GameObject currentPlayer;

    void Start()
    {
        SetCurrentPlayer(EntityManager.GetPlayer());
        currentPlayer.GetComponent<FriendController>().settings.active = true;
        EntityManager.AddOnPlayerChangeAction(() => SetCurrentPlayer(EntityManager.GetPlayer()));

        InputController.GetInputManager().Player.Switch.performed += CheckSwitchPlayer;
        InputController.GetInputManager().Player.Interact.performed += CheckInteraction;
    }

    // private void OnEnable(){
    // 	InputController.GetInputManager().Player.Interact.performed += CheckSwitchPlayer;
    // }

    // private void OnDisable(){
    // 	InputController.GetInputManager().Player.Interact.performed -= CheckSwitchPlayer;
    // }

    private void CheckSwitchPlayer(InputAction.CallbackContext ctx)
    {
        MouseInputUtil.RayCastMousePosition(hit =>
        {
            GameObject hitObj = hit.transform.gameObject;
            if (hitObj.GetComponent<FriendController>() != null && hitObj.GetComponent<FriendController>().settings.active)
            {
                SetCurrentPlayer(hitObj);
            }
        });
    }

    private void SetCurrentPlayer(GameObject player)
    {
        if (currentPlayer != null)
            currentPlayer.GetComponent<FriendController>().settings.isCurrentPlayer = false;

        EntityManager.SetPlayer(player);
        player.GetComponent<FriendController>().settings.isCurrentPlayer = true;
        currentPlayer = player;
    }

    private void CheckInteraction(InputAction.CallbackContext ctx)
    {
        MouseInputUtil.RayCastMousePosition(hit =>
        {
            Interactable interactable = hit.collider.transform.GetComponent<Interactable>();
            if (interactable == null)
                interactable = hit.collider.transform.GetComponentInParent<Interactable>();
            if (interactable != null)
            {
                Interaction interaction = new Interaction(InteractionType.USE, currentPlayer);
                interactable.Interact(interaction);
            }
        });
    }

}
