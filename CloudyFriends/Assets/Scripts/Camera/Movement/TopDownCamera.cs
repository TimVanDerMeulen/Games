using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : CameraMovement
{
    [Serializable]
	public class Settings : CameraMovement.Settings {
        public Transform target;

        public float minDistance;
        public float maxDistance;
    }

    private Settings settings;

    private float currentDistance;

    public TopDownCamera(Settings settings) : base(settings) {
        this.settings = settings;

        currentDistance = settings.minDistance + (settings.maxDistance - settings.minDistance) / 2;
    }

    protected override void PerformUpdate(){
        Vector3 newPos = settings.target.position;
        newPos.y = currentDistance;
        settings.cameraTransform.position = newPos;
    }

}
