using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingCamera : CameraMovement
{
	private Vector3 currentCenter;
		
	public RoamingCamera(Vector3 startingPoint){
		this.currentCenter = startingPoint;
	}
	
    public Vector3 GetCurrentCenter(){
		HandleInput();
		return this.currentCenter;
	}
	
	private void HandleInput() {
		Vector3 movement = GetMovementDirection();
		this.currentCenter += movement.normalized * Time.deltaTime;
	}
	
	private Vector3 GetMovementDirection(){
		//TODO
		return new Vector3(0, 0, 0);
	}
}
