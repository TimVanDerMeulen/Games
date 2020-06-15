using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : ZoomableCameraMovement
{
    [Serializable]
	public class Settings : ZoomableCameraMovement.Settings {

    }

    private Settings settings;

    public TopDownCamera(Settings settings) : base(settings) {
        this.settings = settings;
    }

    protected override void PerformUpdate(){
        base.PerformUpdate();
        
        Vector3 newPos = settings.target.position;
        newPos.y = currentDistance;
        settings.cameraTransform.position = newPos;
    }

}
