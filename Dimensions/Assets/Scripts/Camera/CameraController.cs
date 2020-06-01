using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Targets")]
	public GameObject player;
	public GameObject ground;
	
	[Header("Settings")]
	public float maxDistance;
	public float minDistance;
	
	public Vector3 angle;
	
	private float currentDistance;
	private bool shouldTurnCamera = false;
	
	private Renderer groundRenderer;
	private Vector3 defaultViewDirection;
	
	private CameraMovement cameraMovement;
	
    void Start(){
         groundRenderer = ground.GetComponent<Renderer>();
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
			transform.RotateAround(center, -Vector3.up, 90);
		}
	}
	
	private void centerCameraToGround(Vector3 center){
		transform.position = center + angle * currentDistance;
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
