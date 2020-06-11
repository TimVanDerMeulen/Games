using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomableCameraMovement : CameraMovement
{
    [Serializable]
	public class Settings : CameraMovement.Settings {
        [Header("Zoom Settings")]
        [Tooltip("Limit for zooming out")]
		public float maxDistance;
		[Tooltip("Limit for zooming in")]
		public float minDistance;
        public float zoomSpeed;

        [HideInInspector]
        public float startingDistance;

        [HideInInspector]
        public Transform target;
    }

    private Settings settings;

    protected float currentDistance;

    private bool zoom = false;

    public ZoomableCameraMovement(Settings settings) : base(settings) {
        this.settings = settings;

        AddDefaultRestoreAction(SetZoomDefaults);
        SetZoomDefaults();
    }

    protected virtual Vector3 GetZoomAngle(){
        return settings.cameraTransform.position - settings.target.position;
    }

    protected override void PerformUpdate(){
        if(zoom){
            zoom = false;
            settings.cameraTransform.position = settings.target.position + GetZoomAngle().normalized * currentDistance;
        }
    }
    protected override void OnActivate(){
        base.OnActivate();

        InputController.GetInputManager().Camera.Zoom.performed += Zoom;
    }
	protected override void OnDeactivate(){
        base.OnDeactivate();

        InputController.GetInputManager().Camera.Zoom.performed -= Zoom;
    }

    private void Zoom(InputAction.CallbackContext ctx){
        if(ctx.ReadValue<float>() < 0)
            currentDistance += settings.zoomSpeed * Time.deltaTime;
        else
            currentDistance -= settings.zoomSpeed * Time.deltaTime;

        if(currentDistance < settings.minDistance)
            currentDistance = settings.minDistance;
        else if(currentDistance > settings.maxDistance)
            currentDistance = settings.maxDistance;
        zoom = true;
    }

    private void SetZoomDefaults(){
        if(settings.startingDistance >= settings.minDistance)
            currentDistance = settings.startingDistance;
        else
            currentDistance = settings.minDistance + (settings.maxDistance - settings.minDistance) / 2;
            
        if(settings.zoomSpeed == 0)
            settings.zoomSpeed = (settings.maxDistance - settings.minDistance) / 30f;
    }
}
