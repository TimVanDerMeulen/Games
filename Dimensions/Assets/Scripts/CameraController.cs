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
	
	private bool shouldTurnCamera = false;
	
	private Renderer groundRenderer;
	private Vector3 defaultViewDirection;
	
    void Start(){
         groundRenderer = ground.GetComponent<Renderer>();
		 defaultViewDirection = transform.forward;
    }

    void Update(){
		float distance = minDistance;
//		Debug.Log(groundRenderer.bounds);
		
		//float groundWidth = groundRenderer.bounds.width;
		//float groundHeight = groundRenderer.bounds.height;
		
		//Debug.Log(groundWidth + " x " + groundHeight);
		
		Vector3 center;
		if(distance > maxDistance){
			distance = maxDistance;
			center = player.transform.position;
			center.y = 0;
		}else{
			center = groundRenderer.bounds.center;
		}
		
		handleRotation(center);
	}
	
	public void turnCamera(){
		this.shouldTurnCamera = true;
	}
	
	private void handleRotation(Vector3 center){
		if (shouldTurnCamera){
			shouldTurnCamera = false;
			transform.RotateAround(center, -Vector3.up, 90);
		}
	}
}
