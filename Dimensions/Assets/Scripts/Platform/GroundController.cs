using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	
	private GameObject[,] groundField;
		
	private UnityEngine.Random random = new UnityEngine.Random();
	private List<GameObject> platforms;

    void Start()
    {
		groundField = new GameObject[maxSize, maxSize];
    }

    void Update()
    {
        
    }
	
	private void createRandomPlatform(){
		
	}
	
	private void createPlatform(GameObject platform, int x, int z){
		Instantiate(platform, new Vector3(x * tileSize, 0, z * tileSize), Quaternion.identity);
	}

}
