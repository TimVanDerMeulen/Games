using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
	private static EntityManager INSTANCE;
	
	[Header("Player")]
	public GameObject player;
	
	[Header("Mob Capacity")]
	public int hostile; // TODO make use of mob caps
	public int neutral;
	public int friendly;
	
	private Dictionary<Type, List<GameObject>> mobs = new Dictionary<Type, List<GameObject>>();
	
	/*
		public static Methods (spawn / kill mobs etc.)
	*/
	
	public static GameObject GetPlayer(){
		return INSTANCE.player;
	}
	
	public static GameObject TryToSpawnMob(GameObject mob, Vector3 pos) {
		//TODO prevent spawning when mob cap already full
		GameObject mobInstance = CreateInstanceOf(mob, pos);
		return mobInstance;
	}
	
	public static List<GameObject> GetMobsNear(Vector3 point, float radius, Type type){
		List<GameObject> mobsOfType = new List<GameObject>();
		if(type == null)
			foreach(KeyValuePair<Type, List<GameObject>> pair in INSTANCE.mobs)
				mobsOfType.AddRange(pair.Value);
		else
			mobsOfType.AddRange(INSTANCE.mobs[type]);
		
		return mobsOfType.FindAll(mob => Vector3.Distance(point, mob.transform.position) <= radius);
	}
	
	public static GameObject CreateInstanceOf(GameObject prefab, Vector3 pos){
		return EntityManager.CreateInstanceOf(prefab, pos, Quaternion.identity, null);
	}
	
	public static GameObject CreateInstanceOf(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent){
		return INSTANCE.InternalCreateInstanceOf(prefab, pos, rot, parent);
	}
	
	/*
		private delegate Methods
	*/
	
	private GameObject InternalCreateInstanceOf(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent){
		if(parent != null)
			return Instantiate(prefab, pos, rot, parent);
		
		return Instantiate(prefab, pos, Quaternion.identity);
	}
	
	/*
		Internal Logic
	*/
	
    void Start()
    {
		if(INSTANCE != null)
			throw new InvalidOperationException("EntityManager may only be created once!");
		
		INSTANCE = this;
    }

    void Update()
    {
        //TODO maybe try to get rid of enimies etc. by making them jump off or die...
    }
}
