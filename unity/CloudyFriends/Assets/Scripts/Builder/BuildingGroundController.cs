using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGroundController : MonoBehaviour
{
   [Serializable]
	public class Settings {

        public HexFieldGenerator.Settings groundLayoutSettings;

        public GameObject groundLayerPrefab;

        [Range(0, 1)]
		public float previewTransparency;

        public bool previewActive = false;
	}
	
	public Settings settings;

	private HexFieldGenerator hexFieldGenerator;

    private List<GroundLayerController> groundLayers = new List<GroundLayerController>();
    private int currentLayer = 0;

    private GameObject currentPreviewPrefab, currentPreview;

    void Awake(){
        hexFieldGenerator = new HexFieldGenerator(settings.groundLayoutSettings);

        AddLayer(0);

        Preview(settings.groundLayoutSettings.hex, transform.position);
    }

    void Update() {
        if(currentPreview.activeSelf != settings.previewActive)
            currentPreview.SetActive(settings.previewActive);
    }
	
    public void Preview(GameObject prefab, Vector3 pos){
        Vector3 possiblePosition = hexFieldGenerator.GetClosestHexCenter(pos);
        possiblePosition.y = groundLayers[currentLayer].settings.height;

        if(currentPreview == null || currentPreview != prefab){
            currentPreviewPrefab = prefab;
            Destroy(currentPreview);
            GameObject previewHex = Instantiate(prefab, possiblePosition, transform.rotation  * prefab.transform.rotation);
            ConfigurePreviewLook(previewHex);
            currentPreview = previewHex;
        } else {
            currentPreview.transform.position = possiblePosition;
        }        
    }

    private void ConfigurePreviewLook(GameObject preview){
        Renderer r = preview.GetComponent<Renderer>();

        Color color = r.material.color;
		color.a = settings.previewTransparency;
		r.material.color = color;
    }

    private void AddLayer(int index){
        GameObject initialLayer = Instantiate(settings.groundLayerPrefab, transform.position, transform.rotation, transform);
        groundLayers.Insert(index, initialLayer.GetComponent<GroundLayerController>());
    } 

}
