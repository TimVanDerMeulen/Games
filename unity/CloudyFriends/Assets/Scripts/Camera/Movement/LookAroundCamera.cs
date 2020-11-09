using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundCamera : CameraMovement
{
	[Serializable]
	public class Settings : CameraMovement.Settings {
		[Header("First Person Settings")]
		public float rotationSpeed;
		
		//	target transforms for movement
		[HideInInspector]
		public Transform target;
	}
	
	private Settings settings;
	
    public LookAroundCamera(Settings settings) : base(settings){
		this.settings = settings;
	}

	protected override void OnActivate(){
		base.OnActivate();

		// set first person view to head in prev view direction
		var viewDirection = settings.cameraTransform.forward;
		viewDirection.y=0;
		settings.cameraTransform.rotation = Quaternion.LookRotation(viewDirection, Vector3.up);

		Cursor.lockState = CursorLockMode.Locked;
		InputController.GetInputManager().Player.Disable();
		InputController.GetInputManager().Camera.Disable();
		InputController.GetInputManager().Camera.ToggleMode.Enable();
	}

	protected override void OnDeactivate(){
		base.OnDeactivate();

		Cursor.lockState = CursorLockMode.None;

		InputController.GetInputManager().Player.Enable();
		InputController.GetInputManager().Camera.Enable();
	}
	
    protected override void PerformUpdate(){
		Vector3 position = settings.target.position;
		
		settings.cameraTransform.position = position;
		
		HandleInput();
	}
	
	private void HandleInput(){
		Vector2 cursorDelta = InputController.GetInputManager().Cursor.Movement.ReadValue<Vector2>();
		cursorDelta *= Time.deltaTime;
		Quaternion turnHorizontal = Quaternion.AngleAxis(cursorDelta.x * settings.rotationSpeed, Vector3.up);
		Quaternion turnVertical = Quaternion.AngleAxis(cursorDelta.y * settings.rotationSpeed * (-1), Vector3.right);
		settings.cameraTransform.rotation = turnHorizontal * settings.cameraTransform.rotation * turnVertical;	
	}
	
}
