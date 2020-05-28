using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

[Serializable]
public class GroundGenerator {
	
	/*
	Ground Settings
	*/
	[Header("Dimensions")]
	public int maxSize;
	public int tileSize; // always squared even if tile itself isn't!
	
	[Header("Prefab Settings")]
	public GameObject defaultCloud;
	[Tooltip("All possibleclouds")]
	public List<GameObject> allRandomPlatforms;
	
	[Header("Generation Settings")]
	[Tooltip("Time in seconds between platform spawns")]
	public float generationRate;
	[Tooltip("Slow down generation rate depending on amount of existing platforms")]
	public bool dynamicGenerationRate;
	
	public float spawnHeight;
	
	// x, y, height in cloud grid
	private List<Vector3> freeSpaces;
	
	private Transform groundRootTransform;
	
	private float timer = 0.0f;
	
	private List<PlatformSpawnAnimation> spawnAnimations = new List<PlatformSpawnAnimation>();
	
	public void SetGroundRootTransform(Transform groundRootTransform){
		this.groundRootTransform = groundRootTransform;
	}
	
	public void Init(){
		freeSpaces = new List<Vector3>();
		
		int amount = maxSize / tileSize;
		float halfSize = maxSize / 2;
		
		for(int i=0;i<amount;i++){
			for(int e=0;e<amount;e++){
				FreeSpace(new Vector3(i * tileSize - halfSize, 0, e * tileSize - halfSize));
			}
		}
	}
	
	public void GeneratePlatform(){
		GameObject platform = GetRandomPlatform();
		Vector3 targetPosition = GetFreePlatformPosition();
		
		GameObject platformInstance = GeneratePlatform(platform, targetPosition);
		AddToAnimations(platformInstance, targetPosition);
	}
	
	public GameObject GeneratePlatform(GameObject prefab, Vector3 targetPosition){
		// make platform move straight up to target position
		Vector3 spawnPos = GetSpawnPos(targetPosition);
		
		GameObject platform = EntityManager.CreateInstanceOf(prefab, spawnPos, Quaternion.identity, groundRootTransform);
		if(freeSpaces.Contains(targetPosition)){
			freeSpaces.Remove(targetPosition);
		
			// setup freeing of space after selfdestruct
			PlatformController platformController = platform.GetComponent<PlatformController>();		
			platformController.AddSelfDestructListener(() => this.FreeSpace(targetPosition));
		}
		
		return platform;
	}
	
	public void AddToAnimations(GameObject target, Vector3 destination){
		spawnAnimations.Add(new PlatformSpawnAnimation(target, GetSpawnPos(destination), destination));
	}
	
	public GameObject GetRandomPlatform(){
		return ((Random.value * 5 > 2) ? defaultCloud : allRandomPlatforms[(int)(Random.value * allRandomPlatforms.Count)]);
	}
	
	public Vector3 GetFreePlatformPosition(){
		return freeSpaces[(int)(Random.value * freeSpaces.Count)];
	}
	
	public bool HasFreeSpace(){
		return freeSpaces.Count > 0;
	}
	
	public void Update(){
		GenerateIfRateTimePassed();			
		HandleAnimations();
	}
	
	public void UpdateNavMesh(GameObject platform){
		PlatformController controller = platform.GetComponent<PlatformController>();
		controller.UpdateNavMesh();
	}
	
	private void GenerateIfRateTimePassed(){
		timer += Time.deltaTime;
		if(timer > generationRate && (!dynamicGenerationRate || timer > generationRate + maxSize - freeSpaces.Count)){
			timer = 0.0f;
			if(HasFreeSpace()){
				GeneratePlatform();
			}
		}
	}
	
	private void HandleAnimations(){
		List<PlatformSpawnAnimation> temp = new List<PlatformSpawnAnimation>();
		temp.AddRange(spawnAnimations);
		
		GameObject target = null;
		bool updateMesh = false;
		foreach(PlatformSpawnAnimation anim in temp){
			anim.Animate();
			if(anim.Done()){
				spawnAnimations.Remove(anim);
				updateMesh = true;
				target = anim.target;
			}
		}
		if(updateMesh)
			UpdateNavMesh(target);
	}
	
	private Vector3 GetSpawnPos(Vector3 destination){
		return new Vector3(destination.x, spawnHeight, destination.z);
	}
	
	private void FreeSpace(Vector3 pos){
		// maybe recalc y to randomise height again
		//pos.y = 0; //TODO add variance in height
		freeSpaces.Add(pos);
	}
	
	private class PlatformSpawnAnimation {
		
		public GameObject target;
		public Vector3 spawnPos;
		public Vector3 destination;
		private bool done = false;
		
		public PlatformSpawnAnimation(GameObject target, Vector3 spawnPos, Vector3 destination){
			this.target = target;
			this.spawnPos = spawnPos;
			this.destination = destination;
		}
		
		public void Animate(){
			Vector3 currentPos = target.transform.position;
			float distance = Vector3.Distance(currentPos, destination);
			
			if(distance == 0f){
				target.GetComponent<PlatformController>().DestinationReached();
				done = true;
			}
			
			float move = distance;
			if(move > 0.1)
				move = distance / Vector3.Distance(spawnPos, destination);
			
			target.transform.position += (destination - currentPos).normalized * move;
		}
		
		public bool Done(){
			return done;
		}
	}

}