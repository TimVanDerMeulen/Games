using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HighLighterController : MonoBehaviour
{
    public Shader shader;

    public float lastDuration;

    private List<Type> types = new List<Type>() {typeof(Interactable)};

    private List<HighlightedObject> highlightedObjects = new List<HighlightedObject>();

    void Update() {
       RaycastHit hit;
		
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		//Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 60f, true);
		if (Physics.Raycast(ray, out hit)) 
			foreach(Type t in types)
                if(hit.transform.GetComponent(t) != null)
                    Register(hit);
        
        List<HighlightedObject> temp = new List<HighlightedObject>(highlightedObjects);
        foreach(HighlightedObject h in temp) {
            h.UpdateTimeAlive();
            if(h.GetTimeAlive() >= lastDuration) {
                highlightedObjects.Remove(h);
                h.ApplyPrevShader();
            }
        }

    }

    private void Register(RaycastHit hit){
        Renderer renderer = hit.transform.GetComponent<Renderer>();

        List<HighlightedObject> temp = new List<HighlightedObject>(highlightedObjects);
        foreach(HighlightedObject h in temp) 
            if(h.IsTargetRenderer(renderer)){
                h.ResetTimeAlive();
                return;
            }
        highlightedObjects.Add(new HighlightedObject(hit.transform.GetComponent<Renderer>(), shader));

    }

    private class HighlightedObject {
        private Renderer renderer;

        private float timeAlive = 0f;

        private Shader prevShader;

        public HighlightedObject(Renderer renderer, Shader highlightShader){
            this.renderer = renderer;

            prevShader = renderer.material.shader;
            renderer.material.shader = highlightShader;
        }

        public float GetTimeAlive(){
            return timeAlive;
        }

        public void UpdateTimeAlive(){
            timeAlive += Time.deltaTime;
        }

        public void ApplyPrevShader(){
            renderer.material.shader = prevShader;
        }

        public bool IsTargetRenderer(Renderer renderer){
            return this.renderer == renderer;
        }

        public void ResetTimeAlive(){
            timeAlive = 0f;
        }

    }
}
