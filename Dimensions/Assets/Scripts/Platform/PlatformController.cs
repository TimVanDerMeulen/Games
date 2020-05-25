using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour, Destructable
{
	
	private Rigidbody rigidbody;
	
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //rigidbody.AddForce(new Vector3(1,0,1).normalized);
    }
	
	public void Destroy(){
		
	}
	
}
