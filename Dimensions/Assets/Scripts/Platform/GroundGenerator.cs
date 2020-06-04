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
	[Tooltip("How many Platforms may appear?")]
	public int maxAmount;
	[Tooltip("How big is the area the platforms can spawn in?")]
	public float maxSize;
	
	[Header("Prefab Settings")]
	[Tooltip("Base of all the platforms")]
	public GameObject defaultCloud;
	[Tooltip("All possibleclouds")]
	public List<GameObject> allRandomPlatforms;
	
	[Header("Generation Settings")]
	[Tooltip("Time in seconds between platform spawns")]
	public float generationRate;
	[Tooltip("Slow down generation rate depending on amount of existing platforms")]
	public bool dynamicGenerationRate;
	
	public int amountOfGroups;
	[Range(0,1)]
	public float chanceToRisePlatform;
	public float risenPlatformHeight;
	
	public float spawnHeight;
	
	/*
		privates
	*/
	
	private List<PlatformGroup> platformGroups;
	private int maxFreeSpaces;
	
	private Transform groundRootTransform;
	
	private float timer = 0.0f;
	
	private List<PlatformSpawnAnimation> spawnAnimations = new List<PlatformSpawnAnimation>();
	
	private Vector3 tileSize;
	
	private Action updateNavMesh;
	
	public void SetUpdateNavMeshAction(Action updateNavMesh){
		this.updateNavMesh = updateNavMesh;
	}
	
	public void SetGroundRootTransform(Transform groundRootTransform){
		this.groundRootTransform = groundRootTransform;
	}
	
	public void Init(){
		Renderer platformRenderer = defaultCloud.GetComponent<Renderer>();
		tileSize = platformRenderer.bounds.size;
		
		CalculateFreeSpaces();
	}
		
		
		//private int t = 0;
	public void Update(){
		// spawn groups at once
		//timer += Time.deltaTime;
		//
		//if(timer > 5 && t<platformGroups.Count){
		//	while(platformGroups[t].HasFreeSpace()){
		//		GameObject platform = GetRandomPlatform();
		//		Vector3 targetPosition = platformGroups[t].GetRandomFreeSpace();
		//		GameObject platformInstance = GeneratePlatform(platform, targetPosition);
		//		AddToAnimations(platformInstance, targetPosition);
		//	}
		//	t++;
		//	timer = 0f;
		//}
		
		GenerateIfRateTimePassed();			
		HandleAnimations();
	}
	
	public void GeneratePlatform(){
		if(!HasFreeSpace())
			return;
		
		GameObject platform = GetRandomPlatform();
		Vector3 targetPosition = GetFreePlatformPosition();
		GameObject platformInstance = GeneratePlatform(platform, targetPosition);
		AddToAnimations(platformInstance, targetPosition);
		
		timer = 0.0f;
	}
	
	public GameObject GeneratePlatform(GameObject prefab, Vector3 targetPosition){
		// make platform move straight up to target position
		Vector3 spawnPos = GetSpawnPos(targetPosition);
		
		GameObject platform = EntityManager.CreateInstanceOf(prefab, spawnPos, prefab.transform.rotation, groundRootTransform);
		PlatformGroup pg = GetGroupForSpace(targetPosition);
		if(pg.IsFree(targetPosition)){
			pg.UseSpace(targetPosition, platform);
		
			// setup freeing of space after selfdestruct
			PlatformController platformController = platform.GetComponent<PlatformController>();		
			platformController.AddSelfDestructListener(() => pg.FreeSpace(targetPosition));
		}
		
		return platform;
	}
	
	public void AddToAnimations(GameObject target, Vector3 destination){
		spawnAnimations.Add(new PlatformSpawnAnimation(target, GetSpawnPos(destination), destination));
	}
	
	public GameObject GetRandomPlatform(){
		return (Random.Range(0, 5) > 2) ? defaultCloud : allRandomPlatforms[Random.Range(0, allRandomPlatforms.Count)];
	}
	
	public Vector3 GetFreePlatformPosition(){
		return GetFreePlatformPosition(platformGroups);
	}
	
	public bool HasFreeSpace(){
		foreach(PlatformGroup pg in platformGroups)
			if(pg.HasFreeSpace())
				return true;
		return false;
	}
		
	public void HandleAnimations(){
		List<PlatformSpawnAnimation> temp = new List<PlatformSpawnAnimation>();
		temp.AddRange(spawnAnimations);
		
		GameObject target = null;
		bool updateMesh = false;
		foreach(PlatformSpawnAnimation anim in temp){
			anim.Animate();
			if(anim.Done()){
				spawnAnimations.Remove(anim);
				updateMesh = true;
			}
		}
		if(updateMesh && this.updateNavMesh != null)
			this.updateNavMesh();
	}
	
	private Vector3 GetFreePlatformPosition(List<PlatformGroup> available){
		if(available.Count == 0)
			throw new InvalidOperationException("No free space available!");
		
		PlatformGroup pg = platformGroups[Random.Range(0, available.Count)];
		if(pg.HasFreeSpace())
			return pg.GetRandomFreeSpace();
		
		List<PlatformGroup> a = new List<PlatformGroup>();
		a.AddRange(available);
		a.Remove(pg);
		return GetFreePlatformPosition(a);
	}
	
	private void GenerateIfRateTimePassed(){
		int freeSpacesCount = GetFreeSpacesCount();
		if(maxAmount <= (maxFreeSpaces - freeSpacesCount))
			return;
		
		timer += Time.deltaTime;
		if(timer > generationRate && (!dynamicGenerationRate || timer > (generationRate + maxAmount - (maxFreeSpaces - freeSpacesCount)))){
			timer = 0.0f;
			if(HasFreeSpace()){
				GeneratePlatform();
			}
		}
	}
	
	private int GetFreeSpacesCount(){
		int res = 0;
		foreach(PlatformGroup pg in platformGroups)
			res += pg.GetFreeSpacesCount();
		return res;
	}
	
	private Vector3 GetSpawnPos(Vector3 destination){
		return new Vector3(destination.x, spawnHeight, destination.z);
	}
	
	// calculating hex positioning with grouping
	private void CalculateFreeSpaces(){
		List<Vector3> availableSpacesToGroup = CalcAllPossibleSpaces();
		maxFreeSpaces = availableSpacesToGroup.Count;
		
		List<PlatformGroup> groups = GroupSpaces(availableSpacesToGroup);
		platformGroups = groups;
	}
	
	private List<PlatformGroup> GroupSpaces(List<Vector3> availableSpacesToGroup){
		List<PlatformGroup> groups = new List<PlatformGroup>();
		
		if(amountOfGroups < 1)
			amountOfGroups = 1;
		
		int maxBaseGroupSize = (int)(availableSpacesToGroup.Count/amountOfGroups);
		
		Dictionary<PlatformGroup, bool> platformRisen = new Dictionary<PlatformGroup, bool>();
		
		float maxDistance = 1.5f * tileSize.magnitude;
		
		for(float j=0;j<amountOfGroups;j++){
			PlatformGroup pg = new PlatformGroup();
			groups.Add(pg);
			platformRisen.Add(pg, Random.value < chanceToRisePlatform);
		
			int size = 1;
			Vector3 center = availableSpacesToGroup[Random.Range(0, availableSpacesToGroup.Count)];
			availableSpacesToGroup.Remove(center);
			for(int i=0; i < availableSpacesToGroup.Count; i++){
				if(maxBaseGroupSize <= size)
						break;
				
				if(Vector3.Distance(center, availableSpacesToGroup[i]) <= maxDistance){
					Vector3 v = availableSpacesToGroup[i];
					availableSpacesToGroup.Remove(v);
					i--;
					
					v.y = platformRisen[pg] ? risenPlatformHeight : 0;
					pg.AddSpace(v);
					size++;
				}
			}
		}
		
		while(maxAmount > (maxFreeSpaces - availableSpacesToGroup.Count) && availableSpacesToGroup.Count > 0){
			//TODO not do bs
			Vector3 space = availableSpacesToGroup[0];
			int randomGroupIndex = Random.Range(0, groups.Count);
			groups[randomGroupIndex].AddSpace(space);
			availableSpacesToGroup.Remove(space);
		}
		
		return groups;
	}
	
	private List<Vector3> CalcAllPossibleSpaces(){
		float amountX = maxSize / tileSize.x;
		float amountZ = maxSize / (tileSize.z * 3/4);
		
		List<Vector3> availableSpacesToGroup = new List<Vector3>();
		
		for(float i=0;i<amountX;i++){
			for(float e=0;e<amountZ;e++){
				availableSpacesToGroup.Add(new Vector3(i * tileSize.x - (maxSize/2) + (e%2)*(tileSize.x / 2), 0, e * tileSize.z * 3/4 - (maxSize/2)));
			}
		}
		return availableSpacesToGroup;
	}
	
	private PlatformGroup GetGroupForSpace(Vector3 space){
		foreach(PlatformGroup pg in platformGroups)
			if(pg.ContainsSpace(space))
				return pg;
		throw new ArgumentException("Space not found: " + space);
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
	
	private class PlatformGroup {
		private List<Vector3> freeSpaces = new List<Vector3>();
		private Dictionary<Vector3, GameObject> usedSpaces = new Dictionary<Vector3, GameObject>();
		
		public void AddSpace(Vector3 space){
			if(ContainsSpace(space))
				return;
			
			freeSpaces.Add(space);
		}
		
		public bool HasFreeSpace(){
			return freeSpaces.Count > 0;
		}
		
		public int GetFreeSpacesCount(){
			return freeSpaces.Count;
		}
		
		public Vector3 GetRandomFreeSpace(){
			if(!HasFreeSpace())
				throw new InvalidOperationException("No free space available in this group!");
			
			int index = Random.Range(0, freeSpaces.Count);
			return freeSpaces[index];
		}
		
		public void UseSpace(Vector3 space, GameObject platform){
			if(usedSpaces.ContainsKey(space))
				throw new InvalidOperationException("This space is already in use!");
			
			if(!freeSpaces.Contains(space))
				throw new InvalidOperationException("This space is not available in this group!");
			
			freeSpaces.Remove(space);
			usedSpaces.Add(space, platform);
		}
		
		public void FreeSpace(Vector3 space) {
			if(freeSpaces.Contains(space))
				return; // nothing to do
			
			if(usedSpaces.ContainsKey(space))
				throw new InvalidOperationException("This space is not available in this group!");
			
			usedSpaces[space].GetComponent<PlatformController>().SelfDestruct();
			freeSpaces.Add(space);
		}
		
		public bool ContainsSpace(Vector3 space){
			return freeSpaces.Contains(space) || usedSpaces.ContainsKey(space);
		}
		
		public bool IsFree(Vector3 space){
			return freeSpaces.Contains(space);
		}
		
	}

}