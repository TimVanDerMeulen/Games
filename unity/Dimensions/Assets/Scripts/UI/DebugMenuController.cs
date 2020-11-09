using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenuController : MonoBehaviour
{
	public GameObject debugPanel;
	
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
			debugPanel.active = !debugPanel.active;
    }
}
