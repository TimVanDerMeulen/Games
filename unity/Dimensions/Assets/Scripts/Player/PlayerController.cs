﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
	
	private Camera camera;
	private NavMeshAgent agent;
		    
	void Start() {		
		camera = Camera.main;
	
		agent = GetComponent<NavMeshAgent>();
	}
	
	void Update() {
		if (Input.GetMouseButtonDown(1)) {
			RaycastHit hit;

			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				agent.SetDestination(hit.point);
				return;
			}
		}		
	}

}