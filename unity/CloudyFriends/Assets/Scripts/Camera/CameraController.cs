using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Movement Settings")]
	public FollowCamera.Settings followCameraSettings;
	public LookAroundCamera.Settings lookAroundCameraSettings;
	
	private GameObject player;
	private List<CameraMovement> cameraMovements;
	
    void Start(){
		player = EntityManager.GetPlayer();
		EntityManager.AddOnPlayerChangeAction(() => {
				ResetPlayerSettings();
				player = EntityManager.GetPlayer();
				InitMovement();
			});
		
		InitMovement();

		InputController.GetInputManager().Camera.ToggleMode.performed += ctx => ToggleCameraMode();
	}

    void Update(){
		// HandleInput();
		
		foreach(CameraMovement movement in cameraMovements)
			movement.Update();
	}
	
	private void InitMovement(){
		cameraMovements = new List<CameraMovement>();
		
		followCameraSettings.target = player.transform;	 
		followCameraSettings.cameraTransform = transform;	 
		followCameraSettings.startingDistance = Vector3.Distance(player.transform.position, transform.position);
		cameraMovements.Add(new FollowCamera(followCameraSettings));
		 
		lookAroundCameraSettings.target = player.transform.Find("Head");	 
		lookAroundCameraSettings.cameraTransform = transform;	
		cameraMovements.Add(new LookAroundCamera(lookAroundCameraSettings));
		
		ChangeToCamera(followCameraSettings);
	}
	
	private void RenderPlayer(bool render){
		player.GetComponent<Renderer>().enabled = render;
	}
	
	private void ResetPlayerSettings(){
		RenderPlayer(true);
	}

	private void ToggleCameraMode(){
		if(followCameraSettings.active){
			ChangeToCamera(lookAroundCameraSettings);
			RenderPlayer(false);
		} else {
			ChangeToCamera(followCameraSettings);
			RenderPlayer(true);
		}
	}
	
	private void ChangeToCamera(CameraMovement.Settings cameraSetting) {
		followCameraSettings.active = false;
		lookAroundCameraSettings.active = false;
		
		cameraSetting.active = true;
	}
	
}
