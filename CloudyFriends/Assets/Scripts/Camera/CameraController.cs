using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Target")]
	public GameObject player;
	
	[Header("Other Settings")]
	public FollowCamera.Settings followCameraSettings;
	public LookAroundCamera.Settings lookAroundCameraSettings;
	
	private List<CameraMovement> cameraMovements;
	private CameraMovement thridPersonView;
	private CameraMovement firstPersonView;
	
    void Start(){
		InitMovement();
	}

    void Update(){
		HandleInput();
		
		foreach(CameraMovement movement in cameraMovements)
			movement.Update();
	}
	
	private void InitMovement(){
		cameraMovements = new List<CameraMovement>();
		
		followCameraSettings.target = player.transform;	 
		followCameraSettings.cameraTransform = transform;	 
		thridPersonView = new FollowCamera(followCameraSettings);
		cameraMovements.Add(thridPersonView);
		 
		lookAroundCameraSettings.target = player.transform.Find("Head");	 
		lookAroundCameraSettings.cameraTransform = transform;	
		lookAroundCameraSettings.active = false;	
		firstPersonView = new LookAroundCamera(lookAroundCameraSettings);
		cameraMovements.Add(firstPersonView);
	}
	
	private void HandleInput(){
		if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
			lookAroundCameraSettings.active = true;  			
			followCameraSettings.active = false;  			
        }
		if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
			lookAroundCameraSettings.active = false;  			
			followCameraSettings.active = true;  
		}
	}
	
}
