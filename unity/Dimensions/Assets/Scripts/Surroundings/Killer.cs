using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
	public void OnCollisionEnter(Collision collision){
		if(collision.gameObject != null)
			Destroy(collision.gameObject);
	}
}
