using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexFieldGenerator
{
	[Serializable]
	public class Settings {
		[Tooltip("How big is the area the platforms can spawn in?")]
		public Rect size;
		[Tooltip("Prefab ob hex")]
		public GameObject hex;
	}
	
	private Settings settings;
	
	private Rect tileSize;
	private List<Vector3> spaces;
	
	public HexFieldGenerator(Settings settings){
		this.settings = settings;
		
		Refresh();
	}
	
	public int GetSpacesCount(){
		return spaces.Count;
	}
	
	public Rect GetTileSize(){
		return tileSize;
	}
	
	public List<Vector3> GetSpaces(){
		return new List<Vector3>(spaces);
	}
	
    public void Refresh()
    {
		Renderer hexRenderer = settings.hex.GetComponent<Renderer>();
        var bounds = hexRenderer.bounds.size;
		tileSize = new Rect();
		tileSize.width = bounds.x;
		tileSize.height = bounds.z;
		
		this.spaces = CalcAllPossibleSpaces();
    }
	
	private List<Vector3> CalcAllPossibleSpaces(){
		float amountX = settings.size.width / tileSize.width;
		float amountZ = settings.size.height / (tileSize.height * 3/4);
		
		List<Vector3> availableSpaces = new List<Vector3>();
		
		for(float i=0;i<amountX;i++){
			for(float e=0;e<amountZ;e++){
				availableSpaces.Add(new Vector3(i * tileSize.width - (settings.size.width/2) + (e%2)*(tileSize.width / 2), 0, e * tileSize.height * 3/4 - (settings.size.height/2)));
			}
		}
		return availableSpaces;
	}
	
}
