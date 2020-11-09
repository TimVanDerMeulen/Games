using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil
{
    public static Bounds GetBoundsWithParent(GameObject parent)
    {
        Bounds bounds = parent.GetComponent<Renderer>().bounds;
        foreach(var r in parent.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }

}
