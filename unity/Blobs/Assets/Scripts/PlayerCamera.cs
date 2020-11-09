using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    private GameObject body;
    private Vector3 initialPosition;

    void Start()
    {
        body = this.transform.parent.Find("Body").gameObject;
        initialPosition = transform.position - body.transform.position;
    }

    /*
    void Update()
    {

        Vector3 position = new Vector3(body.transform.position.x + initialPosition.x, body.transform.position.y + initialPosition.y, body.transform.position.z + initialPosition.z);
        transform.position = position;
    }
    */

    public float desiredHeight = 10f;

    public float flightSmoothTime = 10f;
    public float maxFlightspeed = 10f;
    public float flightAcceleration = 1f;

    public float levelingSmoothTime = 0.5f;
    public float maxLevelingSpeed = 10000f;
    public float levelingAcceleration = 2f;

    private Vector3 flightVelocity = Vector3.zero;
    private float heightVelocity = 0f;

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        float currentHeight = position.y;

        if ((bool)body.transform && flightAcceleration > float.Epsilon)
        {
            position = Vector3.SmoothDamp(position, body.transform.position + initialPosition, ref flightVelocity, flightSmoothTime / flightAcceleration, maxFlightspeed, flightAcceleration);
        }

        if (levelingAcceleration > float.Epsilon)
        {
            float targetHeight = Terrain.activeTerrain.SampleHeight(position) + desiredHeight;

            position.y = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, levelingSmoothTime / levelingAcceleration, maxLevelingSpeed, levelingAcceleration);
        }

        transform.position = position;
    }
}
