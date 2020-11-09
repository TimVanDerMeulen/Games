using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class PlainGroundController : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public GameObject platform;

        [Tooltip("How big is the area the platforms can spawn in?")]
        public Rect size;

        public bool generate = true;
    }

    public Settings settings;

    private Rect tileSize;
    private List<Vector3> spaces = new List<Vector3>();

    void Update()
    {
        if (settings.generate && settings.platform != null)
        {
            settings.generate = false;

            ReGenerate();
            List<Vector3> spacesTmp = new List<Vector3>(this.spaces);

            Clear();
            while (spacesTmp.Count > 0)
            {
                Vector3 spawnPos = spacesTmp[Random.Range(0, spacesTmp.Count)];
                GameObject hexField = EntityManager.CreateInstanceOf(settings.platform, spawnPos, settings.platform.transform.rotation, transform);
                spacesTmp.Remove(spawnPos);
                Debug.Log(spacesTmp.Count);
            }
        }
    }

    private void ReGenerate()
    {
        var bounds = GameObjectUtil.GetBoundsWithParent(settings.platform).size;
        tileSize = new Rect();
        tileSize.width = bounds.x;
        tileSize.height = bounds.z;

        if (tileSize.width == 0 || tileSize.height == 0)
            return;

        this.spaces = CalcAllPossibleSpaces();
    }

    private void Clear()
    {
        var children = transform.Cast<Transform>().ToList();
        foreach (Transform t in children)
            DestroyImmediate(t.gameObject);
    }

    private List<Vector3> CalcAllPossibleSpaces()
    {
        float amountX = settings.size.width / tileSize.width;
        float amountZ = settings.size.height / tileSize.height;

        List<Vector3> availableSpaces = new List<Vector3>();

        for (int i = 0; i < amountX; i++)
        {
            for (int e = 0; e < amountZ; e++)
            {
                Vector3 center = new Vector3(i * tileSize.width - settings.size.width / 2f, 0, e * tileSize.height - settings.size.height / 2f);
                availableSpaces.Add(center);
            }
        }
        return availableSpaces;
    }

}
