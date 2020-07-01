using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GroundController : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();

        UpdateNavMesh();
    }

    void Update()
    {

    }

    public void UpdateNavMesh()
    {
        navMeshSurface.BuildNavMesh();
        //PlayerController.RefreshNavMeshAgent();	
    }

}
