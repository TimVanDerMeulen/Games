using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : CameraMovement
{
	[Serializable]
	public class Settings : CameraMovement.Settings {
		[Header("Positioning")]
		[Tooltip("Limit for zooming out")]
		public float maxDistance;
		[Tooltip("Limit for zooming in")]
		public float minDistance;
		[Tooltip("The angle to the target when starting")]
		public Vector3 defaultAngle;
		
		[Header("Turning")]
		public float rotationSpeed;
		
		//	target transforms for movement
		[HideInInspector]
		public Transform target;
		[HideInInspector]
		public Transform cameraTransform;
	}
	
	private Settings settings;

	private Quaternion currentRotation = Quaternion.identity;	
	private float currentDistance;
	private Vector3 currentAngle;
	
	private bool turningCamera = false;
		
	public FollowCamera(Settings settings) : base(settings){
		this.settings = settings;
		
		ResetDefaults();
	}
	
    protected override void PerformUpdate(){
		Vector3 currentCenter = settings.target.position;
		
		TurnAroundCenter(currentCenter);
		CenterTarget(currentCenter);
	}

	protected override void OnActivate(){
		InputController.GetInputManager().Camera.Rotate.performed += OnRotate;
	}
	protected override void OnDeactivate(){
		InputController.GetInputManager().Camera.Rotate.performed -= OnRotate;
	}
	
	private void OnRotate(InputAction.CallbackContext ctx){
		// InputActionPhase phase = ctx.phase;
		// switch (phase)
        // {
        //     case InputActionPhase.Started:
		// 		turningCamera = true;
		// 		break;
        //     case InputActionPhase.Canceled:
        //     case InputActionPhase.Performed:
		// 	case InputActionPhase.Disabled:
        //         turningCamera = false;
        //         break;
		// 	default:
		// 		break;
        // }
		turningCamera = !turningCamera;
	}
	
	private void TurnAroundCenter(Vector3 center){
		if(turningCamera){
			Vector2 cursorDelta = InputController.GetInputManager().Cursor.Movement.ReadValue<Vector2>();
			cursorDelta *= Time.deltaTime;
			Quaternion turnHorizontal = Quaternion.AngleAxis(cursorDelta.x * settings.rotationSpeed, Vector3.up);
			Quaternion turnVertical = Quaternion.AngleAxis(cursorDelta.y * (-1) * settings.rotationSpeed, Vector3.right);
			currentAngle = turnHorizontal * turnVertical * currentAngle;
			
			//currentRotation *= Quaternion.Euler(0, settings.rotationSpeed * Time.deltaTime, 0); 
		}
	}
	
	private void CenterTarget(Vector3 center){
		settings.cameraTransform.position = center + currentRotation * (currentAngle * currentDistance);
		settings.cameraTransform.LookAt(settings.target);
	}
	
	private void ResetDefaults(){
		this.currentDistance = this.settings.minDistance;
		this.currentAngle = this.settings.defaultAngle;
	}

}
