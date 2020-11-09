using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WalkMode : MovementMethods {
	
	[Header("Walk Base Settings")]
	public float WalkSpeed = 0.4f;
	public float JumpHeight = 2;
	
	[Header("Walk Controls")]
	public KeyCode BUTTON_MOVE_FORWARD = KeyCode.W;
	public KeyCode BUTTON_MOVE_BACKWARD = KeyCode.S;
	public KeyCode BUTTON_MOVE_LEFT = KeyCode.A;
	public KeyCode BUTTON_MOVE_RIGHT = KeyCode.D;
	public KeyCode BUTTON_MOVE_UP = KeyCode.Space;
	//public KeyCode BUTTON_MOVE_DOWN = KeyCode.LeftShift;
	
	private Transform targetTransform;
	private Rigidbody targetRigidbody;
	
	private Transform cameraTransform;
	private Quaternion groundRotation;
	private CharacterController characterController;
	
	public void SetTarget(GameObject target){
		this.targetTransform = target.transform;
		this.targetRigidbody = target.GetComponent<Rigidbody>();
		this.characterController = target.GetComponent<CharacterController>();
	}
	
	public void SetCamera(Transform camera){
		this.cameraTransform = camera;
	}
	
	public Quaternion GetRotation() {
		RaycastHit hit;
		if (Physics.Raycast(this.targetTransform.position, -this.targetTransform.up, out hit, this.characterController.height))
		{
			Vector3 viewDirection =  new Vector3(this.targetTransform.forward.x, 0, this.targetTransform.forward.z);
			Quaternion viewRotation = Quaternion.LookRotation(viewDirection);
			Quaternion groundAngle = Quaternion.FromToRotation(Vector3.up, hit.normal);
			if(groundRotation == null || this.groundRotation != groundAngle){
				this.groundRotation = groundAngle;
				return groundAngle * viewRotation;
			}
		}		
		return this.targetTransform.rotation;
	}
	
	public void ApplyMovement(){
		Vector3 direction = GetMovementDirection() * Time.deltaTime;
		
		if(direction != Vector3.zero){
			direction.y -= 20 * Time.deltaTime;
			
			Vector3 prevPos = this.targetTransform.position;
	
			characterController.Move(direction);
			
			Vector3 newPos = this.targetTransform.position;
					
			this.targetTransform.rotation = Quaternion.LookRotation(newPos - prevPos);
		}
		
		// jump
		if (Input.GetKey(BUTTON_MOVE_UP))
		{
			characterController.Move(Vector3.up  * CalculateJumpVerticalSpeed());
		}
	}
	
	private Vector3 GetMovementDirection(){
		Vector3 force = Vector3.zero;
		if (Input.GetKey(BUTTON_MOVE_FORWARD))
		{
			force += this.cameraTransform.forward;
		}
		if (Input.GetKey(BUTTON_MOVE_BACKWARD))
		{
			force += this.cameraTransform.forward  * -1;
		}
		if (Input.GetKey(BUTTON_MOVE_RIGHT))
		{
			force += this.cameraTransform.right;
		}
		if (Input.GetKey(BUTTON_MOVE_LEFT))
		{
			force += this.cameraTransform.right  * -1;
		}
		
		// reset any falsy height change
		force.y = 0;
		
		return force.normalized * WalkSpeed;
	}
	
	private float CalculateJumpVerticalSpeed()
	{
		return Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(Physics.gravity.y));
	}
	
}