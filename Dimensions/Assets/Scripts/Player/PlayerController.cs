using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
	
	public static void RefreshNavMeshAgent(){
		walkMotion.Refresh();
	}
	
	private Camera camera;
	private NavMeshAgent agent;
	
	private static WalkMotion walkMotion = new WalkMotion();
	    
	void Start() {		
		camera = Camera.main;
	
		agent = GetComponent<NavMeshAgent>();
	}
	
	void Update() {
		if (Input.GetMouseButtonDown(1)) {
			RaycastHit hit;

			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				walkMotion.SetDestinationForAgent(hit.point, agent);
				return;
			}
		}		
	}

}

class WalkMotion {
	
	private Vector3 destination;
	private NavMeshAgent agent;
	
	public void SetDestinationForAgent(Vector3 destination, NavMeshAgent agent){
		this.destination = destination;
		this.agent = agent;
		
		Refresh();
	}
	
	public void Refresh(){
		agent.SetDestination(destination);
	}
	
}
