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

	private bool lastActiveState = false;
	
	public CameraMovement(Settings settings){
		this.settings = settings;
	}
	
    public void Update(){
		if(settings.active != lastActiveState){
			if(settings.active)
				OnActivate();
			else
				OnDeactivate();
			lastActiveState = settings.active;
		}

		if(settings.active)
			PerformUpdate();
	}
	
	protected abstract void PerformUpdate();
	protected virtual void OnActivate(){}
	protected virtual void OnDeactivate(){}
}
