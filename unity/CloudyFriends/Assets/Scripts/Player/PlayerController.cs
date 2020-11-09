using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
	
	protected NavMeshAgent agent;
		    
	public virtual void Start() {			
		agent = GetComponent<NavMeshAgent>();

		InputController.GetInputManager().Player.Movement.performed += CheckMovementClick;
	}

	// private void OnEnable(){
	// 	InputController.GetInputManager().Player.Movement.performed += CheckMovementClick;
	// }

	// private void OnDisable(){
	// 	InputController.GetInputManager().Player.Movement.performed -= CheckMovementClick;
	// }

	public virtual void Update(){}
	
	private void CheckMovementClick(InputAction.CallbackContext ctx){
		RaycastHit hit;
		
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		//Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 60f, true);
		if (Physics.Raycast(ray, out hit)) {
			MoveTo(hit);
			return;
		}
	}
	
	protected virtual void MoveTo(RaycastHit hit) {
		agent.SetDestination(hit.point);
	}

}