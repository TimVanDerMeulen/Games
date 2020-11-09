using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	private static InputController INSTANCE;
	
	private InputManager inputManager;
	
	public InputController(){
		if(INSTANCE != null)
			throw new InvalidOperationException("InputController may only be created once!");
		
		INSTANCE = this;
	}
	
	public static InputManager GetInputManager(){
		return INSTANCE.inputManager;
	}

	void Awake(){
		inputManager = new InputManager();
		inputManager.Enable();
	}
}
