using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class GroundController : MonoBehaviour
{
	[Header("Resources")]
	public string PLATFORM_PREFAB_FOLDER;
	
	/*
	 Ground Settings
	*/
	[Header("Dimensions")]
	public int maxSize;
	public int tileSize; // always squared even if tile itself isn't!
	
	[Header("Start Platforms")]
	public PlatformController[] startPlatforms;
			
	private UnityEngine.Random random = new UnityEngine.Random();

    void Start()
    {
		NavMesh.RemoveAllNavMeshData();
		startPlatforms[0].GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    void Update()
    {
        
    }
	
	private void createRandomPlatform(){
		
	}
	
	private void createPlatform(NavMeshSurface platform, int x, int z){
		NavMeshSurface instance = Instantiate(platform, new Vector3(x * tileSize, 0, z * tileSize), Quaternion.identity);
		
	}

}
