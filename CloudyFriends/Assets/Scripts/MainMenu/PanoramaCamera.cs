using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanoramaCamera : MonoBehaviour
{
	[Header("Target")]
	public GameObject target;
	
	[Header("Settings")]
	public float speed;
	[Range(0,180)]
	public int maxVerticalRoation;
	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.transform.position, -Vector3.up, Time.deltaTime * speed);
		
		transform.LookAt(target.transform);
    }
}
