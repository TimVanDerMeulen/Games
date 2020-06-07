using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

public class GroundController : MonoBehaviour
{
	[Header("Generation Settings")]
	public GroundGenerator.Settings groundGeneratorSettings;

	private GroundGenerator groundGenerator;

	private bool started = false;
	private NavMeshSurface navMeshSurface;

    void Start()
    {
		groundGeneratorSettings.groundRootTransform = transform;
		groundGeneratorSettings.updateNavMesh = () => this.UpdateNavMesh();
		groundGenerator = new GroundGenerator(groundGeneratorSettings);
		
		navMeshSurface = GetComponent<NavMeshSurface>();
    }

    void Update()
    {
		if(!started){
			//still handle manually added platform animations
			groundGenerator.HandleAnimations();
			return;
		}
		
		groundGenerator.Update();
    }
	
	public void Init(){
		groundGenerator.Init();
		
		GameObject startPlatform = CreateStartPlatform();
		PlatformController platformController = startPlatform.GetComponent<PlatformController>();
		
		if(EntityManager.GetPlayer() != null)
			platformController.AddDestinationReachedListener(() => {
				EntityManager.GetPlayer().transform.position = startPlatform.transform.position + new Vector3(0,3,0);
				groundGenerator.SpawnGroup(startPlatform.transform.position);
			});
		
	}
	
	public void StartGeneration(){
		started = true;
	}
	
	public void CreateRandomCloud(){
		groundGenerator.GeneratePlatform();
	}
	
	private GameObject CreateStartPlatform(){
		Vector3 pos = groundGenerator.GetFreePlatformPosition();
		GameObject platform = groundGenerator.GeneratePlatform(groundGeneratorSettings.hex, pos);
		groundGenerator.AddToAnimations(platform, pos);
		
		return platform;
	}
	
	private void UpdateNavMesh(){
		navMeshSurface.BuildNavMesh();
		//PlayerController.RefreshNavMeshAgent();	
	}

}
