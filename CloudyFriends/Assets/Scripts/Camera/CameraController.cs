using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Other Settings")]
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
		cameraMovements.Add(new FollowCamera(followCameraSettings));
		 
		lookAroundCameraSettings.target = player.transform.Find("Head");	 
		lookAroundCameraSettings.cameraTransform = transform;	
		cameraMovements.Add(new LookAroundCamera(lookAroundCameraSettings));
		
		ChangeToCamera(followCameraSettings);
	}
	
	private void HandleInput(){
		if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
			ChangeToCamera(lookAroundCameraSettings);		
			RenderPlayer(false);	

			// set first person view to head in prev view direction
			var viewDirection = lookAroundCameraSettings.cameraTransform.forward;
			viewDirection.y=0;
			lookAroundCameraSettings.cameraTransform.rotation = Quaternion.LookRotation(viewDirection, Vector3.up);			
        }
		if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
			ChangeToCamera(followCameraSettings);  
			RenderPlayer(true);
		}
	}
	
	private void RenderPlayer(bool render){
		player.GetComponent<Renderer>().enabled = render;
	}
	
	private void ResetPlayerSettings(){
		RenderPlayer(true);
	}
	
	private void ChangeToCamera(CameraMovement.Settings cameraSetting) {
		followCameraSettings.active = false;
		lookAroundCameraSettings.active = false;
		
		cameraSetting.active = true;
	}
	
}
