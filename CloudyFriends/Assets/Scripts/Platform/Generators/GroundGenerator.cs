using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

public class GroundGenerator {
	
	[Serializable]
	public class Settings : HexFieldGenerator.Settings {
		/*
			Ground Settings
		*/
		[Header("Dimensions")]
		[Tooltip("How many Platforms may appear?")]
		public int maxAmount;
		
		[Header("Prefab Settings")]
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
		
		[HideInInspector]
		public Transform groundRootTransform;
		
		[HideInInspector]
		public Action updateNavMesh;
	}
	
	private Settings settings;
	
	private HexFieldGenerator hexFieldGenerator;
	
	private List<PlatformGroup> platformGroups;
	private int maxFreeSpaces;
	
	private float timer = 0.0f;
	
	private List<PlatformSpawnAnimation> spawnAnimations = new List<PlatformSpawnAnimation>();
	
	public GroundGenerator(Settings settings){
		this.settings = settings;
	}
	
	public void Init(){
		hexFieldGenerator = new HexFieldGenerator(settings);
		
		CalculateFreeSpaces();
	}
		
	public void Update(){		
		GenerateIfRateTimePassed();			
		HandleAnimations();
	}
	
	public void GeneratePlatform(){
		if(!HasFreeSpace())
			return;
		
		Vector3 targetPosition = GetFreePlatformPosition();
		GeneratePlatform(targetPosition);
	}
	
	public void GeneratePlatform(Vector3 targetPosition){
		if(!HasFreeSpace())
			return;
		
		GameObject platform = GetRandomPlatform();
		GameObject platformInstance = GeneratePlatform(platform, targetPosition);
		AddToAnimations(platformInstance, targetPosition);
		
		timer = 0.0f;
	}
	
	public GameObject GeneratePlatform(GameObject prefab, Vector3 targetPosition){
		// make platform move straight up to target position
		Vector3 spawnPos = GetSpawnPos(targetPosition);
		
		GameObject platform = EntityManager.CreateInstanceOf(prefab, spawnPos, prefab.transform.rotation, settings.groundRootTransform);
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
		return (Random.Range(0, 5) > 2) ? settings.hex : settings.allRandomPlatforms[Random.Range(0, settings.allRandomPlatforms.Count)];
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
		if(updateMesh && settings.updateNavMesh != null)
			settings.updateNavMesh();
	}
	
	public void SpawnGroup(Vector3 space){
		PlatformGroup pg = GetGroupForSpace(space);
		
		while(pg.HasFreeSpace()){
			GeneratePlatform(pg.GetRandomFreeSpace());
		}
	}
	
	private Vector3 GetFreePlatformPosition(List<PlatformGroup> available){
		if(available.Count < 1)
			throw new InvalidOperationException("No free space available!");
		
		PlatformGroup pg = available[Random.Range(0, available.Count)];
		if(pg.HasFreeSpace())
			return pg.GetRandomFreeSpace();
		
		List<PlatformGroup> a = new List<PlatformGroup>();
		a.AddRange(available);
		a.Remove(pg);
		return GetFreePlatformPosition(a);
	}
	
	private void GenerateIfRateTimePassed(){
		int freeSpacesCount = GetFreeSpacesCount();
		int maxFreeSpaces = hexFieldGenerator.GetSpacesCount();
		if(settings.maxAmount <= (maxFreeSpaces - freeSpacesCount))
			return;
		
		timer += Time.deltaTime;
		if(timer > settings.generationRate && (!settings.dynamicGenerationRate || timer > (settings.generationRate + settings.maxAmount - (maxFreeSpaces - freeSpacesCount)))){
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
		return new Vector3(destination.x, settings.spawnHeight, destination.z);
	}
	
	// calculating hex positioning with grouping
	private void CalculateFreeSpaces(){
		List<Vector3> availableSpacesToGroup = hexFieldGenerator.GetSpaces();
		
		List<PlatformGroup> groups = GroupSpaces(availableSpacesToGroup);
		platformGroups = groups;
	}
	
	private List<PlatformGroup> GroupSpaces(List<Vector3> availableSpacesToGroup){
		List<PlatformGroup> groups = new List<PlatformGroup>();
		
		if(settings.amountOfGroups < 1)
			settings.amountOfGroups = 1;
		
		int maxBaseGroupSize = (int)(availableSpacesToGroup.Count/settings.amountOfGroups);
		
		Dictionary<PlatformGroup, bool> platformRisen = new Dictionary<PlatformGroup, bool>();
		
		float maxDistance = 1.5f * Math.Max(hexFieldGenerator.GetTileSize().width, hexFieldGenerator.GetTileSize().height);
		
		for(float j=0;j<settings.amountOfGroups;j++){
			PlatformGroup pg = new PlatformGroup();
			groups.Add(pg);
			platformRisen.Add(pg, Random.value < settings.chanceToRisePlatform);
		
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
					
					v.y = platformRisen[pg] ? settings.risenPlatformHeight : 0;
					pg.AddSpace(v);
					size++;
				}
			}
		}
		
		while(settings.maxAmount > (hexFieldGenerator.GetSpacesCount() - availableSpacesToGroup.Count) && availableSpacesToGroup.Count > 0){
			//TODO not do bs
			Vector3 space = availableSpacesToGroup[0];
			int randomGroupIndex = Random.Range(0, groups.Count);
			groups[randomGroupIndex].AddSpace(space);
			availableSpacesToGroup.Remove(space);
		}
		
		return groups;
	}
	
	private List<Vector3> CalcAllPossibleSpaces(){
		float amountX = settings.size.width / hexFieldGenerator.GetTileSize().width;
		float amountZ = settings.size.height / (hexFieldGenerator.GetTileSize().height * 3/4);
		
		List<Vector3> availableSpacesToGroup = new List<Vector3>();
		
		for(float i=0;i<amountX;i++){
			for(float e=0;e<amountZ;e++){
				availableSpacesToGroup.Add(new Vector3(i * hexFieldGenerator.GetTileSize().width - (settings.size.width/2) + (e%2)*(hexFieldGenerator.GetTileSize().width / 2), 0, e * hexFieldGenerator.GetTileSize().height * 3/4 - (settings.size.height/2)));
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