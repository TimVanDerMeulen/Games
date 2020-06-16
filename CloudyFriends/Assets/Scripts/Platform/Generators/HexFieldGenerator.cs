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

	private float rowOffset = 3f/4f;
	
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
        var bounds = GameObjectUtil.GetBoundsWithParent(settings.hex).size;
		tileSize = new Rect();
		tileSize.width = bounds.x;
		tileSize.height = bounds.z;
		
		this.spaces = CalcAllPossibleSpaces();
    }

	public Vector3 GetClosestHexCenter(Vector3 pos){
		float diffX = pos.x % tileSize.width;
		float diffY = pos.y % tileSize.height;
		return GetCenterPosFor((int)((pos.y - diffY)/(tileSize.height * rowOffset) + ((diffY < 0) ? (-1) : 1)), (int)((pos.x - diffX)/tileSize.width + ((diffX < 0) ? (-1) : 1)));
	}
	
	private List<Vector3> CalcAllPossibleSpaces(){
		float amountX = settings.size.width / tileSize.width;
		float amountZ = settings.size.height / (tileSize.height * rowOffset);

		List<Vector3> availableSpaces = new List<Vector3>();
		
		for(int i=0;i<amountX;i++){
			for(int e=0;e<amountZ;e++){
				availableSpaces.Add(GetCenterPosFor(e, i));
			}
		}
		return availableSpaces;
	}

	private Vector3 GetCenterPosFor(int row, int col){
		return new Vector3(col * tileSize.width - (settings.size.width/2) + (row%2)*(tileSize.width / 2), 0, row * tileSize.height * rowOffset - (settings.size.height/2));
	}
	
}
