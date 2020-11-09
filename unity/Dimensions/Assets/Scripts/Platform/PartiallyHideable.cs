using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartiallyHideable : MonoBehaviour
{
	[Tooltip("All objects that should be hidden if the player collides with base obj")]
	public List<GameObject> hideOnCollision;
	
	[Tooltip("Are the objects visible at the start?")]
	public bool visible = true;
	
	[Range(0, 1)]
	public float transparency = 0.4f;

	private int colliderCounter = 0;
	
	void Update(){
		if(this.visible == (colliderCounter > 0))
			ToggleVisibility(colliderCounter > 0);
	}
	
    void OnTriggerEnter(Collider collider)
    {
		if (collider.gameObject.tag != "Player")
			return;
		
		//ToggleVisibility(true);
		this.colliderCounter++;
	}
	
	void OnTriggerExit(Collider collider)
    {
		if (collider.gameObject.tag != "Player")
			return;
		
		//ToggleVisibility(false);
		this.colliderCounter--;
	}
	
	public void ToggleVisibility(bool hide){
		if(this.visible == !hide)
			return;// no action needed
		
		foreach(GameObject obj in this.hideOnCollision)
			ToggleVisibility(obj, hide);
	}
	
	private void ToggleVisibility(GameObject gameObject, bool hide) {
		// toggles the visibility of this gameobject and all it's children
		var renderers = gameObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers){
			Color color = r.material.color;
			color.a = hide ? transparency : 1f;
			r.material.color = color;//enabled = !hide;
		}
		this.visible = !hide;
	}
}
