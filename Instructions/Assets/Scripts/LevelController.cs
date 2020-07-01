using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    public UnityEvent onLevelReady;

    private bool started = false;

    public void Update()
    {
        if (started)
            return;
        started = true;
        onLevelReady.Invoke();
        Debug.Log("Level started!");
    }
}
