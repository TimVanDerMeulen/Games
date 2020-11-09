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

    private float minXPos, minZPos;
    private Vector3 basePlatformBounds;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();

        UpdateNavMesh();
        SetupGround();
    }
    private bool test = false;
    void Update()
    {
        if (currentPlatform == null)
            currentPlatform = GetPlayerSpawnerPlatform();
        if (test)
        {
            test = false;
            MovementResult res = TryMoveToNextPlatform(EntityManager.GetPlayer().transform.forward);
            if (res.success)
                EntityManager.GetPlayer().GetComponent<NavMeshAgent>().SetDestination(res.result.transform.position);
        }
    }

    public void UpdateNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    public GameObject GetCurrentPlatform()
    {
        return currentPlatform?.gameObject;
    }

    public void Test()
    {
        test = true;

    }

    public MovementResult TryMoveToNextPlatform(Vector3 direction)
    {
        direction = GetClearDirectionFrom(direction);
        var currentIndex = GroundIndexOf(currentPlatform.gameObject);
        var nextPlatformIndex = new Vector3(currentIndex.x + direction.x, 0, currentIndex.z + direction.z);

        if ((ground.GetLength(0) <= nextPlatformIndex.x || ground.GetLength(1) <= nextPlatformIndex.z) || ground[(int)nextPlatformIndex.x, (int)nextPlatformIndex.z] == null)
            return new MovementResult(false, null); ;

        GameObject nextPlatform = ground[(int)nextPlatformIndex.x, (int)nextPlatformIndex.z];
        if (!currentPlatform.IsConnectedTo(nextPlatform))
            return new MovementResult(false, nextPlatform);

        currentPlatform = nextPlatform.GetComponent<PlatformController>();
        return new MovementResult(true, nextPlatform);
    }

    public class MovementResult
    {
        public bool success { get; private set; }
        public GameObject result { get; private set; }

        public MovementResult(bool success, GameObject result)
        {
            this.success = success;
            this.result = result;
        }
    }

    private void SetupGround()
    {
        basePlatformBounds = GameObjectUtil.GetBoundsWithParent(basePlatform).size;

        minXPos = float.MaxValue;
        float maxXPos = float.MinValue;
        minZPos = float.MaxValue;
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
            Vector3 pos = GroundIndexOf(child.gameObject);
            ground[(int)pos.x, (int)pos.z] = child.gameObject;
        }
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

    private Vector3 GroundIndexOf(GameObject platform)
    {
        Vector3 pos = platform.transform.position;
        int x = (int)((pos.x - minXPos) / basePlatformBounds.x);
        int z = (int)((pos.z - minZPos) / basePlatformBounds.z);
        return new Vector3(x, 0, z);
    }

    private Vector3 GetClearDirectionFrom(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return direction;

        direction = direction.normalized;
        Vector3 closest = direction;
        float closestDistance = float.MaxValue;
        foreach (Vector3 vector in new List<Vector3> { new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1) })
        {
            float distance = Vector3.Distance(vector, direction);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = vector;
            }
        }
        return closest;
    }

}