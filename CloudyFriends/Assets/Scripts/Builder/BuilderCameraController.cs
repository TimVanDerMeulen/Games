using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCameraController : MonoBehaviour
{
     [Serializable]
	public class Settings {
        public Transform target;
        [Header("Movement Settings")]
        public ZoomableCameraMovement.Settings zoomSettings;
    }

    public Settings settings;

    private ZoomableCameraMovement zoomableCamera;
   
    void Awake()
    {
        settings.zoomSettings.target = settings.target;
        settings.zoomSettings.cameraTransform = transform;

        zoomableCamera = new ZoomableCameraMovement(settings.zoomSettings);
    }

    void Update()
    {
        zoomableCamera.Update();
    }
}
