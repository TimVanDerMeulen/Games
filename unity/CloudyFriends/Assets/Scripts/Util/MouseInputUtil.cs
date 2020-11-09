using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputUtil : MonoBehaviour
{

    private static MouseInputUtil INSTANCE;

    public void Awake()
    {
        if (INSTANCE != null)
            throw new InvalidOperationException("MouseInputUtil may only be created once!");
        INSTANCE = this;
    }

    [Serializable]
    public class Settings
    {
        public Camera camera;
    }

    public Settings settings;

    public static Vector2 GetCursorPosition()
    {
        return Mouse.current.position.ReadValue();
    }

    [ObsoleteAttribute("This method is obsolete. Use RayCastMousePosition instead.", false)]
    public static RaycastHit GetCursorHoverOver()
    {
        RaycastHit hit;
        Ray ray = INSTANCE.settings.camera.ScreenPointToRay(GetCursorPosition());
        Physics.Raycast(ray, out hit);
        return hit;
    }

    public static Boolean RayCastMousePosition(Action<RaycastHit> actionOnHit)
    {
        return RayCastMousePosition(actionOnHit, null);
    }

    public static Boolean RayCastMousePosition(Action<RaycastHit> actionOnHit, Action<Ray> actionOnMiss)
    {
        RaycastHit hit;

        Ray ray = INSTANCE.settings.camera.ScreenPointToRay(GetCursorPosition());
        if (Physics.Raycast(ray, out hit))
        {
            if (actionOnHit != null)
                actionOnHit(hit);
            return true;
        }
        if (actionOnMiss != null)
            actionOnMiss(ray);
        return false;
    }
}
