using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class DayCycle : MonoBehaviour
{
	[Header("Sun and Moon")]
	public GameObject sun;
	public GameObject moon;
	
	[Header("Day Cycle Settings")]
	[RangedSlider(0, 24)] 
	public RangedSliderValues dayCycle;
	
	[Header("Updated values")]
	public float currentTime = 8f;
	
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("min:" + dayCycle.minVal);
        Debug.Log("max:" + dayCycle.maxVal);
    }
}
