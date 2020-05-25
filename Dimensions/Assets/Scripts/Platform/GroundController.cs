using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;
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
	
	public GameObject defaultCloud;

    void Start()
    {
		NavMesh.RemoveAllNavMeshData();
		startPlatforms[0].GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    void Update()
    {

    }
	
	public void CreateRandomCloud(){
		CloudController test = defaultCloud.GetComponent<CloudController>();
		test.isStatic = false;
		int xDirPos = (Random.value * 10 < 5) ? 1 : -1;
		int zDirPos = (Random.value * 10 < 5) ? 1 : -1;
		test.direction = new Vector3(xDirPos * Random.value * 100 + 20, 0, zDirPos * Random.value * 100 + 20);
		CreatePlatform(defaultCloud, (int)(-1 * xDirPos * Random.value * (maxSize / tileSize)), (int) (-1 * zDirPos * Random.value * (maxSize / tileSize)));
	}
	
	private void CreatePlatform(GameObject platform, int x, int z){
		GameObject instance = Instantiate(platform, new Vector3(x * tileSize, 0, z * tileSize), Quaternion.identity, transform);
	}

}
