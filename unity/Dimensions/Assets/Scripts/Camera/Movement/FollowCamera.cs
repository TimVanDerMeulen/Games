using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : CameraMovement
{
	private Transform transformToFollow;
		
	public FollowCamera(Transform transformToFollow){
		this.transformToFollow = transformToFollow;
	}
	
    public Vector3 GetCurrentCenter(){
		Vector3 currentCenter = transformToFollow.position;
		//currentCenter.y = 0;
		return currentCenter;
	}

}
