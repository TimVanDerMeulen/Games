using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : PlatformController
{
	public Vector3 direction;
	
    public void Start()
    {
		base.Start();
		
        if(direction == Vector3.zero)
			direction = new Vector3(1,0,1);
    }

    public void Update()
    {
		base.Update();
		if(!base.isStatic)
			transform.position += direction.normalized * 4 * Time.deltaTime;
    }
}
