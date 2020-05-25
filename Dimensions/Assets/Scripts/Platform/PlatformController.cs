using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlatformController : MonoBehaviour, Destructable
{
	
	public bool isStatic = false;
	
	private NavMeshSurface navMeshSurface;
	
    public void Start()
    {
		navMeshSurface = GetComponent<NavMeshSurface>();
        //UpdateNavMesh();
    }

    public void Update()
    {
		
    }
	
	public void OnCollisionEnter(Collision collision){
		if(collision.gameObject != null){
			PlatformController collisionPlatformController = collision.gameObject.GetComponent<PlatformController>();
			if(collisionPlatformController != null){
				bool oldIsStatic = isStatic;
				
				isStatic = true;
				GetComponent<Rigidbody>().isKinematic = true;
				
				if(!oldIsStatic)
					UpdateNavMesh();
			}
		}
	}
	
	public void OnCollisionExit(Collision collision){
		if(true) //TODO check if other collision still up
			UpdateNavMesh();
		else
			Destroy(); 
	}
	
	public void Destroy(){
		//TODO start animation and drop down
	}
	
	private void UpdateNavMesh(){
		NavMesh.RemoveAllNavMeshData();
		navMeshSurface.BuildNavMesh();
		PlayerController.RefreshNavMeshAgent();
	}
	
}
