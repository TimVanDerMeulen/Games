using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerSpawnController : MonoBehaviour
{
    void Start()
    {
        if (EntityManager.GetPlayer() == null)
            return;
        GameObject player = EntityManager.GetPlayer();

        Renderer renderer = GetComponent<Renderer>();
        player.transform.position = renderer.bounds.center;
        player.transform.rotation = transform.rotation;

        if (Application.isPlaying)
            GameObjectUtil.Hide(gameObject);
    }

}
