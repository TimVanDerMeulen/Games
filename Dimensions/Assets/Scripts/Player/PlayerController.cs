using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
	
	private static bool shouldRefreshNavAgent;
	public static void RefreshNavMeshAgent(){
		shouldRefreshNavAgent = true;
	}
	
	private Camera camera;
	private NavMeshAgent agent;
	private Vector3 destination;
	
	private bool walking = false;
		    
	void Start() {		
		camera = Camera.main;
	
		agent = GetComponent<NavMeshAgent>();
	}
	
	void Update() {
		if(shouldRefreshNavAgent) {
			shouldRefreshNavAgent = false;
			RefreshNavMesh();
		}
		if (Input.GetMouseButtonDown(1)) {
			RaycastHit hit;

			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				destination = hit.point;
				walking = true;
				RefreshNavMesh();
				return;
			}
		}		
		if(walking && Vector3.Distance(transform.position, destination) < 0.3)
			walking = false;
	}
	
	private void RefreshNavMesh(){
		if(walking && destination != null)
			agent.SetDestination(destination);
	}

}