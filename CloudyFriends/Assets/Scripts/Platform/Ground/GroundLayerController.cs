using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLayerController : MonoBehaviour
{
    [Serializable]
	public class Settings {
		public float height;
	}
	
	public Settings settings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGameObject(GameObject gameObject, Vector3 pos){
        pos.y = settings.height;
        Instantiate(gameObject, pos, gameObject.transform.rotation, transform);
    }
}
