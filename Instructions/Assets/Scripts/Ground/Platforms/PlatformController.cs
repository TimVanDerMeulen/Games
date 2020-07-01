using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private HashSet<GameObject> connectedPlatforms = new HashSet<GameObject>();

    private HashSet<GameObject> possibleConnects = new HashSet<GameObject>();

    public void OnTriggerEnter(Collider collider)
    {
        PlatformController platformController = collider.gameObject.GetComponentInParent<PlatformController>();
        if (platformController != null)
        {
            platformController.ConnectTo(collider.gameObject);
            ConnectTo(collider.gameObject); // if already there
            possibleConnects.Add(collider.gameObject);
        }
    }

    public void ConnectTo(GameObject obj)
    {
        if (possibleConnects.Contains(obj))
        {
            connectedPlatforms.Add(obj);
            DrawConnection(obj);
        }
    }

    private void DrawConnection(GameObject to)
    {
        Vector3 height = new Vector3(0, 3, 0);

        Debug.DrawLine(transform.position + height, to.transform.position + height, Color.red, 60f);
    }

}
