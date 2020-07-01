using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GroundController : MonoBehaviour
{
    public GameObject basePlatform;

    private NavMeshSurface navMeshSurface;

    private GameObject[,] ground;

    private PlatformController currentPlatform;
    private GameObject player;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();

        UpdateNavMesh();
        SetupGround();
    }

    void Update()
    {
        if (currentPlatform == null)
            SetupPlayer();
    }

    public void UpdateNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    private void SetupGround()
    {
        var basePlatformBounds = GameObjectUtil.GetBoundsWithParent(basePlatform).size;

        float minXPos = float.MaxValue;
        float maxXPos = float.MinValue;
        float minZPos = float.MaxValue;
        float maxZPos = float.MinValue;
        foreach (Transform child in transform)
        {
            float x = child.position.x;
            float z = child.position.z;
            if (minXPos > x)
                minXPos = x;
            if (maxXPos < x)
                maxXPos = x;
            if (minZPos > z)
                minZPos = z;
            if (maxZPos < z)
                maxZPos = z;
        }
        int xLength = (int)((maxXPos - minXPos) / basePlatformBounds.x) + 1;
        int zLength = (int)((maxZPos - minZPos) / basePlatformBounds.z) + 1;
        ground = new GameObject[xLength, zLength];
        foreach (Transform child in transform)
        {
            int x = (int)((child.position.x - minXPos) / basePlatformBounds.x);
            int z = (int)((child.position.z - minZPos) / basePlatformBounds.z);
            ground[x, z] = child.gameObject;
        }
    }

    private void SetupPlayer()
    {
        currentPlatform = GetPlayerSpawnerPlatform();
        player = EntityManager.GetPlayer();
    }

    private PlatformController GetPlayerSpawnerPlatform()
    {
        PlayerSpawnController[] playerSpawner = GetComponentsInChildren<PlayerSpawnController>();
        if (playerSpawner == null || playerSpawner.Length < 1)
            throw new Exception("No player spawner found!");
        PlatformController[] playerSpawnerPlatform = playerSpawner[0].GetComponentsInParent<PlatformController>();
        if (playerSpawner == null || playerSpawnerPlatform.Length < 1)
            throw new Exception("Player spawner has no platform parent!");
        return playerSpawnerPlatform[0];
    }

}