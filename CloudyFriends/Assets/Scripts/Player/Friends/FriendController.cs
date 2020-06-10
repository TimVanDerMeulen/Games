using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : PlayerController
{
	[Serializable]
	public class Settings {
		public bool active = false;
		
		public bool isCurrentPlayer = false;
	}
	
	public Settings settings;
	
	// transform to follow
	private Transform leader;
	
	private float passedTime = 0f;

    public override void Update() {
        if(!settings.active)
			return;
		
		if(settings.isCurrentPlayer) 
			base.Update();
		
		if(leader != null && passedTime > 1) {
			passedTime = 0f;
			
			if(Vector3.Distance(leader.position, transform.position) > 1.5)
				base.agent.SetDestination(leader.position);
			else
				transform.LookAt(leader);
			return;
		}
	
		if(passedTime < 10)
			passedTime += Time.deltaTime;
	}
	
	protected override void MoveTo(RaycastHit hit) {
		if(!settings.isCurrentPlayer)
			return;

		GameObject hitObject = hit.transform.gameObject;
		if(hitObject.GetComponent<FriendController>() && hitObject.transform != transform){
			leader = hitObject.transform;
			return;
		}else{
			leader = null;
		}
		base.MoveTo(hit);
	}
	
}
