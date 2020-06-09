using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		
		HandleInput();
		TurnAroundCenter(currentCenter);
		CenterTarget(currentCenter);
	}
	
	private void HandleInput(){
		if (Input.GetMouseButtonDown(1))
        {
			turningCamera = true;  			
        }
		if (Input.GetMouseButtonUp(1))
        {
			turningCamera = false;
		}
	}
	
	private void TurnAroundCenter(Vector3 center){
		if(turningCamera){
			Quaternion turnHorizontal = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * settings.rotationSpeed, Vector3.up);
			Quaternion turnVertical = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * settings.rotationSpeed, Vector3.right);
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
