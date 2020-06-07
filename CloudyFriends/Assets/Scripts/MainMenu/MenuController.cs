using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public GameObject ground;
	
	private bool init = true;
	
    void Init()
    {
		init = false;
		
        GroundController groundController =  ground.GetComponent<GroundController>();
		groundController.Init();
        groundController.StartGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        if(init)
			Init();
    }
	
	public void StartGame(){
		SceneManager.LoadScene(1);
	}
	
}
