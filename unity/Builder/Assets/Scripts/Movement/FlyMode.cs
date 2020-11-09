using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FlyMode : MovementMethods {

	[Header("Fly Base Settings")]	
	public bool canFly = true;		
	public float FlySpeed = 2;
	public bool invertY = false;
	
	[Header("Fly Controls")]
	public KeyCode BUTTON_MOVE_FORWARD = KeyCode.W;
	public KeyCode BUTTON_MOVE_BACKWARD = KeyCode.S;
	//public KeyCode BUTTON_MOVE_LEFT = KeyCode.A;
	//public KeyCode BUTTON_MOVE_RIGHT = KeyCode.D;
	public KeyCode BUTTON_MOVE_UP = KeyCode.Space;
	public KeyCode BUTTON_MOVE_DOWN = KeyCode.LeftShift;
	
	[Header("Fly Rotation Settings")]
	[Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
	public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

	private Transform targetTransform;
	private Rigidbody targetRigidbody;
	private float targetSize;
	
	public void SetTarget(GameObject target){
		this.targetTransform = target.transform;
		this.targetRigidbody = target.GetComponent<Rigidbody>();
		
		var rt = targetTransform.localScale;
		Debug.Log("scale:" + rt);
		this.targetSize = 1;
	}
			
	public Quaternion GetRotation() {
		var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));
		var mouseSensitivityFactor = this.mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);
		
		Quaternion turnHorizontal = Quaternion.AngleAxis(mouseMovement.x * mouseSensitivityFactor, Vector3.up);
		Quaternion turnVertical = Quaternion.AngleAxis(mouseMovement.y * mouseSensitivityFactor, Vector3.right);
		
		return turnHorizontal * this.targetTransform.rotation * turnVertical;
	}
	
	public void ApplyMovement(){
		this.targetRigidbody.AddRelativeForce(GetRealtiveForce());
		
		// glide when facing within under 90° of velocity dirtection
		Vector3 targetForwardDirection = this.targetTransform.forward.normalized;
		if((targetForwardDirection + this.targetRigidbody.velocity.normalized).magnitude >= 1.2)
			this.targetRigidbody.velocity = targetForwardDirection * this.targetRigidbody.velocity.magnitude;
		
		if (Input.GetKey(BUTTON_MOVE_DOWN))
		{
			this.targetTransform.position -= Vector3.up  * 0.01f * this.targetRigidbody.velocity.magnitude;
		}
	}
	
	private Vector3 GetRealtiveForce(){
		Vector3 force = Vector3.zero;
		
		if (Input.GetKey(BUTTON_MOVE_FORWARD))
		{
			force += Vector3.forward  * FlySpeed * this.targetSize;
		}
		if (Input.GetKey(BUTTON_MOVE_BACKWARD))
		{
			force += Vector3.forward  * -1 * (FlySpeed / 2) * this.targetSize;
		}
		if (Input.GetKey(BUTTON_MOVE_UP))
		{
			force += Vector3.up  * FlySpeed * this.targetSize;
		}
		return force;
	}
	
}