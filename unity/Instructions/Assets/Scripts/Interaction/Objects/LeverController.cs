﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class LeverController : MonoBehaviour, Interactable
{
    public bool defaultState;
    public GameObject on, off;

    [System.Serializable]
    public class OnStateChanged : UnityEvent<bool> { };
    public OnStateChanged onStateChange;

    public bool currentState { get; private set; }

    public new void Awake()
    {
        ShowState(defaultState);
    }

    public void Interact(Interaction interaction)
    {
        ShowState(!currentState);
    }

    private void ShowState(bool state)
    {
        this.currentState = state;
        on.SetActive(state);
        off.SetActive(!state);

        InvokeOnStateChanged();
    }

    private void InvokeOnStateChanged()
    {
        for (int i = 0; i < onStateChange.GetPersistentEventCount(); i++)
        {
            ((MonoBehaviour)onStateChange.GetPersistentTarget(i)).SendMessage(onStateChange.GetPersistentMethodName(i), currentState);
        }
    }

}
