using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class CageController : InteractableController
{
    public GameObject target;

    public GameObject cageBase;

    public UnityEvent onChangeEvent;

    FriendController cagedPlayer;

    public new void Awake()
    {
        base.Awake();

        if (target == null)
            return;

        cagedPlayer = target.GetComponent<FriendController>();

        ResetTargetPos();

        interactionMenu.AddMenuOption(InteractionType.PLAYER, new InteractableMenu.MenuOption("Open", interaction => Open(interaction)));
    }

    private void Open(Interaction interaction)
    {
        if (!(Vector3.Distance(transform.position, interaction.source.transform.position) < 5))
            return;
        gameObject.SetActive(false);

        //cagedPlayer.transform.position = transform.position;
        cagedPlayer.settings.active = true;

        onChangeEvent.Invoke();
    }

    private void ResetTargetPos()
    {
        Renderer cageBaseRenderer = cageBase.GetComponent<Renderer>();
        Bounds cageBaseBounds = cageBaseRenderer.bounds;

        Vector3 targetPos = cageBaseBounds.center;
        targetPos.y = cageBaseBounds.size.y;

        bool targetActive = target.activeSelf;
        target.SetActive(false);
        target.transform.position = targetPos;
        target.SetActive(targetActive);
    }
}
