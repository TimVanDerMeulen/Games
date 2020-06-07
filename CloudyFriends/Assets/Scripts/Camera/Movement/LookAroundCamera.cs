using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundCamera : CameraMovement
{
	[Serializable]
	public class Settings : CameraMovement.Settings {
		public float rotationSpeed;
		
		//	target transforms for movement
		[HideInInspector]
		public Transform target;
		[HideInInspector]
		public Transform cameraTransform;
	}
	
	private Settings settings;
	
    public LookAroundCamera(Settings settings) : base(settings){
		this.settings = settings;
	}
	
    protected override void PerformUpdate(){
		Vector3 position = settings.target.position;
		
		settings.cameraTransform.position = position;
		
		HandleInput();
	}
	
	private void HandleInput(){
		Quaternion turnHorizontal = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * settings.rotationSpeed, Vector3.up);
		Quaternion turnVertical = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * settings.rotationSpeed * (-1), Vector3.right);
		settings.cameraTransform.rotation = turnHorizontal * turnVertical * settings.cameraTransform.rotation;	
	}
	
}
