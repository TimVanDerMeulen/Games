using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public UnityEvent onDoorOpen;

    public bool open;

    private bool isOpen = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            open = false;
            isOpen = true;
            door.SetActive(!isOpen);
            onDoorOpen.Invoke();
        }
    }
}
