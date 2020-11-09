using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EntityManager : MonoBehaviour
{
	private static EntityManager INSTANCE;
	
	[Serializable]
	public class Settings {
		[Header("Player")]
		public GameObject startingPlayer;
	}
	
	public Settings settings;
	
	//private 
	
	private GameObject player;
	private List<Action> onPlayerChange = new List<Action>();
	
	private Dictionary<Type, List<GameObject>> mobs = new Dictionary<Type, List<GameObject>>();
	
	/*
		public static Methods (spawn / kill mobs etc.)
	*/
	
	public static GameObject GetPlayer(){
		return INSTANCE.player;
	}
	
	public static void SetPlayer(GameObject p){
		INSTANCE.SetPlayerInternal(p);
	}
	
	public static void AddOnPlayerChangeAction(Action action){
		INSTANCE.onPlayerChange.Add(action);
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
		return EntityManager.CreateInstanceOf(prefab, pos, prefab.transform.rotation, null);
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
	
	private void SetPlayerInternal(GameObject p){
		if(p == player)
			return;
		
		player = p;
		
		foreach(Action action in onPlayerChange)
			action();
	}
	
	/*
		Internal Logic
	*/
	
    void Awake()
    {
		if(INSTANCE != null)
			throw new InvalidOperationException("EntityManager may only be created once!");
		
		INSTANCE = this;
		player = settings.startingPlayer;
    }

    void Update()
    {
        //TODO maybe try to get rid of enimies etc. by making them jump off or die...
    }
	
}
