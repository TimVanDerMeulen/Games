using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
	
	protected NavMeshAgent agent;
		    
	public virtual void Start() {			
		agent = GetComponent<NavMeshAgent>();
	}
	
	private Ray test;
	
	public virtual void Update() {
		if (Input.GetMouseButtonDown(1)) {
			RaycastHit hit;
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 60f, true);
			if (Physics.Raycast(ray, out hit)) {
				MoveTo(hit);
				return;
			}
		}		
	}
	
	protected virtual void MoveTo(RaycastHit hit) {
		agent.SetDestination(hit.point);
	}

}