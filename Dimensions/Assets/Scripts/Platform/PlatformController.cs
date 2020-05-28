using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlatformController : MonoBehaviour, Destructable
{
	private List<Action> onSelfDestruct = new List<Action>();
	private List<Action> onDestinationReached = new List<Action>();
	
	private NavMeshSurface navMeshSurface;
	
    public void Start()
    {
		navMeshSurface = GetComponent<NavMeshSurface>();
        //UpdateNavMesh();
    }

    public void Update()
    {
		
    }
	
	public void AddSelfDestructListener(Action listener){
		onSelfDestruct.Add(listener);
	}
	
	public void AddDestinationReachedListener(Action listener){
		onDestinationReached.Add(listener);
	}
	
	//public void OnCollisionEnter(Collision collision){
	//	if(collision.gameObject != null){
	//		PlatformController collisionPlatformController = collision.gameObject.GetComponent<PlatformController>();
	//		if(collisionPlatformController != null){
	//			bool oldIsStatic = isStatic;
	//			
	//			isStatic = true;
	//			GetComponent<Rigidbody>().isKinematic = true;
	//			
	//			if(!oldIsStatic)
	//				UpdateNavMesh();
	//		}
	//	}
	//}
	//
	//public void OnCollisionExit(Collision collision){
	//	if(true) //TODO check if other collision still up
	//		UpdateNavMesh();
	//	else
	//		Destroy(); 
	//}
	
	public void DestinationReached(){
		foreach(Action action in onDestinationReached)
			action();
	}
	
	public void SelfDestruct(){
		foreach(Action action in onSelfDestruct)
			action();
			
		//TODO start animation and drop down
	}
	
	public void UpdateNavMesh(){
		NavMesh.RemoveAllNavMeshData();
		navMeshSurface.BuildNavMesh();
		PlayerController.RefreshNavMeshAgent();
	}
	
}
