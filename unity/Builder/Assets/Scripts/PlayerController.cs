using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : ThirdPersonCameraController
{
	[Header("Movement Settings")]
	public WalkMode walkSettings;
	public FlyMode flySettings;
	
	// privates
	private float momentum = 0;
	private Rigidbody rigidbody;
	private MovementMethods currentMovement;
	
	private CharacterController characterController;
	
	/*
	########################## Game Loop ##########################
	*/
	
	void Start(){
		base.Start();
		
		this.ReAdjust();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		
		UpdateCurrentMovement(base.IsFlying());
	}

    void FixedUpdate()
    {		
		this.UpdateRotation();
		this.UpdateMomentum();
    }
	
	/*
	########################## Other ##########################
	*/
		
	public override void ReAdjust(){
		base.ReAdjust();
		
		this.rigidbody = base.target.GetComponent<Rigidbody>();
		this.rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
		
		this.characterController = base.target.GetComponent<CharacterController>();
		
		
		this.flySettings.SetTarget(base.target);
		this.walkSettings.SetTarget(base.target);
		this.walkSettings.SetCamera(this.transform);
	}
	
	protected override void DetectGroundContact(bool groundContact){
		// update super
		base.DetectGroundContact(groundContact);

		// update self
		UpdateCurrentMovement(!groundContact);
		
		// adjust camera snap
		if(groundContact)
			base.snapCamera = false;
		else
			base.snapCamera = true;
		
		// rotate to ground position
		if(groundContact && base.IsFlying()){
			this.rigidbody.velocity = Vector3.zero;
			
			Vector3 viewDirection =  new Vector3(base.target.transform.forward.x, 0, base.target.transform.forward.z);
			Quaternion viewRotation = Quaternion.LookRotation(viewDirection);
			
			base.target.transform.rotation = viewRotation;
			
			RaycastHit hit;
			if (Physics.Raycast(base.target.transform.position, -base.target.transform.up, out hit, 4))
			{
				Quaternion groundAngle = Quaternion.FromToRotation(base.target.transform.up, hit.normal);
				base.target.transform.rotation = groundAngle * viewRotation;
			}
		}
	}
	
	/*
	########################## Private ##########################
	*/
	
	private void UpdateCurrentMovement(bool airborn){		
		currentMovement = ((this.flySettings.canFly && airborn) ? (MovementMethods) flySettings : (MovementMethods) walkSettings);
		
		if(!airborn){
			this.characterController.enabled = true; 
			this.rigidbody.isKinematic = true;
		}else {
			this.characterController.enabled = false; 
			this.rigidbody.isKinematic = false;
		}
	}
	
	private void UpdateRotation()
    {
		if(base.rotateCam) // from superclass
			return;
		
		base.target.transform.rotation = currentMovement.GetRotation();
    }
	
	private void UpdateMomentum(){
		if(this.rigidbody == null)
			return;
		
		currentMovement.ApplyMovement();
	}

}
