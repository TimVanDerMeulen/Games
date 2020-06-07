using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Targets")]
	public GameObject player;
	
	[Header("Settings")]
	public float maxDistance;
	public float minDistance;
	
	public Vector3 angle;
	
	private float currentDistance;
	private Quaternion currentRotation = Quaternion.identity;
	private bool shouldTurnCamera = false;
	
	private Vector3 defaultViewDirection;
	
	private CameraMovement cameraMovement;
	
    void Start(){
		 defaultViewDirection = transform.forward;
		 
		 currentDistance = minDistance;
		 angle = angle.normalized;
		 
		 cameraMovement = new FollowCamera(player.transform);
	}

    void Update(){
		UpdateCameraMovementMethod();
		Vector3 center = cameraMovement.GetCurrentCenter();
		
		handleCameraTurn(center);
		centerCameraToGround(center);
	}
	
	public void turnCamera(){
		this.shouldTurnCamera = true;
	}
	
	private void handleCameraTurn(Vector3 center){
		if (shouldTurnCamera){
			shouldTurnCamera = false;
			currentRotation *= Quaternion.Euler(0, 90, 0);
		}
	}
	
	private void centerCameraToGround(Vector3 center){
		transform.position = center + currentRotation * (angle * currentDistance);
		transform.LookAt(player.transform);
	}
	
	private CameraMovement UpdateCameraMovementMethod(){
		//TODO too much?
		//if(Input.GetKeyDown(KeyCode.R)){
		//	Vector3 startPos = player.transform.position;
		//	startPos.y = 0;
		//	cameraMovement = new RoamingCamera(startPos);
		//}
		//if(Input.GetKeyDown(KeyCode.F))
		//	cameraMovement = new FollowCamera(player.transform);
		return cameraMovement;
	}
}
