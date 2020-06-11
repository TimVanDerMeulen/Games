using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : ZoomableCameraMovement
{
	[Serializable]
	public class Settings : ZoomableCameraMovement.Settings {
		[Header("Rotation Settings")]
		[Tooltip("The angle to the target when starting")]
		public Vector3 defaultAngle;
		
		public float rotationSpeed;
	}
	
	private Settings settings;

	private Quaternion currentRotation = Quaternion.identity;	
	private Vector3 currentAngle;
	
	private bool turningCamera = false;
		
	public FollowCamera(Settings settings) : base(settings){
		this.settings = settings;

		AddDefaultRestoreAction(SetFollowDefaults);
		SetFollowDefaults();
	}
	
    protected override void PerformUpdate(){
		base.PerformUpdate();

		Vector3 currentCenter = settings.target.position;
		
		TurnAroundCenter(currentCenter);
		CenterTarget(currentCenter);
	}

	protected override void OnActivate(){
		base.OnActivate();

		InputController.GetInputManager().Camera.Rotate.performed += OnRotate;
	}
	protected override void OnDeactivate(){
		base.OnDeactivate();
		InputController.GetInputManager().Camera.Rotate.performed -= OnRotate;
	}
	
	private void OnRotate(InputAction.CallbackContext ctx){
		turningCamera = !turningCamera;
	}
	
	private void TurnAroundCenter(Vector3 center){
		if(turningCamera){
			Vector2 cursorDelta = InputController.GetInputManager().Cursor.Movement.ReadValue<Vector2>();
			cursorDelta *= Time.deltaTime;
			Quaternion turnHorizontal = Quaternion.AngleAxis(cursorDelta.x * settings.rotationSpeed, Vector3.up);
			Quaternion turnVertical = Quaternion.AngleAxis(cursorDelta.y * (-1) * settings.rotationSpeed, Vector3.right);
			currentAngle = turnHorizontal * turnVertical * currentAngle;
		}
	}
	
	private void CenterTarget(Vector3 center){
		settings.cameraTransform.position = center + currentRotation * (currentAngle * currentDistance);
		settings.cameraTransform.LookAt(settings.target);
	}

	private void SetFollowDefaults(){
		this.currentAngle = this.settings.defaultAngle;
	}

}
