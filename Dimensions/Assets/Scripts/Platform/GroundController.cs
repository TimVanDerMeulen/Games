using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;
using System;

public class GroundController : MonoBehaviour
{
	[Header("Generation Settings")]
	public GroundGenerator groundGenerator = new GroundGenerator();

	private bool started = false;

    void Start()
    {
		
    }

    void Update()
    {
		if(!started)
			return;
		
		groundGenerator.Update();
    }
	
	public void Init(){
		groundGenerator.SetGroundRootTransform(transform);
		groundGenerator.Init();
		
		GameObject startPlatform = CreateStartPlatform();
		PlatformController platformController = startPlatform.GetComponent<PlatformController>();
		platformController.AddDestinationReachedListener(() => EntityManager.GetPlayer().transform.position = startPlatform.transform.position + new Vector3(0,3,0));
	}
	
	public void StartGeneration(){
		started = true;
	}
	
	public void CreateRandomCloud(){
		groundGenerator.GeneratePlatform();
	}
	
	private GameObject CreateStartPlatform(){
		Vector3 pos = groundGenerator.GetFreePlatformPosition();
		GameObject platform = groundGenerator.GeneratePlatform(groundGenerator.defaultCloud, pos);
		groundGenerator.AddToAnimations(platform, pos);
		
		return platform;
	}

}
