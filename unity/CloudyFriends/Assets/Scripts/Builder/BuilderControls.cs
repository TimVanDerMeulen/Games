using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderControls : MonoBehaviour
{
    private static BuilderControls INSTANCE;

    public void Awake(){
        if(INSTANCE != null)
			throw new InvalidOperationException("BuilderControls may only be created once!");
		
		INSTANCE = this;
    }

    public static void AddBuildModeChangedListener(Action listener){
        INSTANCE.onBuildModeChanged.Add(listener);
    }

    public GameObject ground;
    private bool buildingMode = false;

    private List<Action> onBuildModeChanged = new List<Action>();
    private BuildingGroundController builderGroundController;

    void Start()
    {
        builderGroundController = ground.GetComponent<BuildingGroundController>();   
    }

    void Update()
    {
        if(buildingMode)
            builderGroundController.Preview(builderGroundController.settings.groundLayoutSettings.hex, MouseInputUtil.GetCursorHoverOver().point);
    }

    public void SetBuildingMode(bool buildingMode) {
        bool oldVal = this.buildingMode;
        this.buildingMode = buildingMode;

        if(oldVal != buildingMode)
            foreach(Action a in onBuildModeChanged)
                a();
    }

}
