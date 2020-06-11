using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CameraMovement
{
	[Serializable]
	public class Settings{
		[Header("Basic Settings")]
		[Tooltip("Should this movement be applied on update?")]
		public bool active = true;
		[HideInInspector]
		public Transform cameraTransform;
	}
	
	private Settings settings;

	private bool lastActiveState = false;

	private List<Action> restoreDefaultActions = new List<Action>();
	
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

	public void SetDefaults(){
		List<Action> temp = new List<Action>(restoreDefaultActions);
		foreach(Action a in temp)
			a();
	}

	protected void AddDefaultRestoreAction(Action restoreDefaultAction){
		this.restoreDefaultActions.Add(restoreDefaultAction);
	}
	
	protected abstract void PerformUpdate();
	protected virtual void OnActivate(){}
	protected virtual void OnDeactivate(){}
}
