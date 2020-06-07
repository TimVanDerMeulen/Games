using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CameraMovement
{
	[Serializable]
	public class Settings{
		[Tooltip("Should this movement be applied on update?")]
		public bool active = true;
	}
	
	private Settings settings;
	
	public CameraMovement(Settings settings){
		this.settings = settings;
	}
	
    public void Update(){
		if(settings.active)
			PerformUpdate();
	}
	
	protected abstract void PerformUpdate();
}
