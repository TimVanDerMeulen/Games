using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	public GameObject ground;

	private bool once = true;

    // Update is called once per frame
    void Update()
    {
		if(once){
			once = false;
			StartGroundGeneration();
		}
    }
	
	private void StartGroundGeneration(){
		GroundController groundController = ground.GetComponent<GroundController>();
		groundController.Init();
		groundController.StartGeneration();
	}
}
