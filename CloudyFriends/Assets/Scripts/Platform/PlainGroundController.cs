using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

[ExecuteInEditMode]
public class PlainGroundController : MonoBehaviour
{
	[Serializable]
	public class Settings : HexFieldGenerator.Settings {
		public bool generate = true;
	}
	
	public Settings settings;
	
	private HexFieldGenerator hexFieldGenerator;
	
	private List<GameObject> field;

    void Update()
    {
        if(settings.generate && settings.hex != null){
			settings.generate = false;
			
			if(hexFieldGenerator == null)
				hexFieldGenerator = new HexFieldGenerator(settings);
			
			if(field != null && field.Count > 0)
				foreach(GameObject obj in field)
					DestroyImmediate(obj);
			
			List<Vector3> spaces = hexFieldGenerator.GetSpaces();
			field = new List<GameObject>();
			
			while(spaces.Count > 0){
				Vector3 spawnPos = spaces[Random.Range(0, spaces.Count)];
				GameObject hexField = EntityManager.CreateInstanceOf(settings.hex, spawnPos, settings.hex.transform.rotation, transform);
				field.Add(hexField);
				spaces.Remove(spawnPos);
			}
		}
    }
	
	public void ReGenerate(){
		hexFieldGenerator.Refresh();
		settings.generate = true;
	}

}
