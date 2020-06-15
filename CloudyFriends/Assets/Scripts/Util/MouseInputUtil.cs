using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputUtil : MonoBehaviour
{

    private static MouseInputUtil INSTANCE;

    public void Awake(){
        if(INSTANCE != null)
            throw new InvalidOperationException("MouseInputUtil may only be created once!");
        INSTANCE = this;
    }

    [Serializable]
	public class Settings {
        public Camera camera;
    }

    public Settings settings;

    public static Vector2 GetCursorPosition(){
        return Mouse.current.position.ReadValue();
    }

    public static RaycastHit GetCursorHoverOver(){
        RaycastHit hit;
		Ray ray = INSTANCE.settings.camera.ScreenPointToRay(GetCursorPosition());
		Physics.Raycast(ray, out hit); 
        return hit;
    }
}
