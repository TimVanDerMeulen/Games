using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour, Interactable
{
    public bool state;

    void Update()
    {
        
    }

    public void Interact(Interaction interaction){
        Debug.Log("Lever interact: " + interaction);
    }
}
