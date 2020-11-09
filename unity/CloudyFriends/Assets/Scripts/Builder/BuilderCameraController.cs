using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCameraController : MonoBehaviour
{
    //  [Serializable]
	// public class Settings {
    //     public Transform target;
    //     [Header("Movement Settings")]
    //     public TopDownCamera.Settings zoomSettings;
    //     public FollowCamera.Settings rotateSettings;
    // }

    // public Settings settings;

    // private List<CameraMovement> cameraMovements = new List<CameraMovement>();
    // private List<ZoomableCameraMovement.Settings> cameraSettings = new List<ZoomableCameraMovement.Settings>();
   
    // void Awake()
    // {
    //     SetupCameraSettings();
    //     settings.zoomSettings.target = settings.target;
    //     settings.zoomSettings.cameraTransform = transform;
    //     cameraMovements.Add(new TopDownCamera(settings.zoomSettings));

    //     settings.rotateSettings.target = settings.target;
    //     settings.rotateSettings.cameraTransform = transform;
    //     cameraMovements.Add(new FollowCamera(settings.rotateSettings));

    //     BuilderControls.AddBuildModeChangedListener();
    // }

    // void Update()
    // {
    //     UpdateCameraMovements();
    // }

    // private void UpdateCameraMovements(){
    //     foreach(CameraMovement m in cameraMovements)
    //         m.Update();
    // }

    // private void SwitchToCamera(CameraMovement.Settings toActivate){
    //     foreach(CameraMovement.Settings s in cameraSettings)
    //         s.active = false;
    //     toActivate.active = true;
    // }

    // private void SetupCameraSettings(){
    //      foreach(ZoomableCameraMovement.Settings s in cameraSettings){
    //         s.target = settings.target;
    //         s.cameraTransform = transform;
    //     }
    // }

}
