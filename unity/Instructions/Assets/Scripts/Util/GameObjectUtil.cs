using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil
{
    public static Bounds GetBoundsWithParent(GameObject parent)
    {
        Bounds bounds = parent.GetComponent<Renderer>().bounds;
        foreach (var r in parent.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }
    public static void Hide(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            Hide(child.gameObject);
        }
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
            renderer.enabled = false;
    }

}
