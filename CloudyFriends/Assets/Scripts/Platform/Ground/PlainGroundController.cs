using System;
using System.Linq;
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

    void Update()
    {
        if(settings.generate && settings.hex != null){
			settings.generate = false;
			
			if(hexFieldGenerator == null)
				hexFieldGenerator = new HexFieldGenerator(settings);
			
			List<Vector3> spaces = hexFieldGenerator.GetSpaces();
			
			Clear();
			while(spaces.Count > 0){
				Vector3 spawnPos = spaces[Random.Range(0, spaces.Count)];
				GameObject hexField = EntityManager.CreateInstanceOf(settings.hex, spawnPos, settings.hex.transform.rotation, transform);
				spaces.Remove(spawnPos);
			}
		}
    }
	
	public void ReGenerate(){
		hexFieldGenerator.Refresh();
		settings.generate = true;
	}

	private void Clear(){
		var children = transform.Cast<Transform>().ToList();
		foreach(Transform t in children)
			DestroyImmediate(t.gameObject);
	}

}
