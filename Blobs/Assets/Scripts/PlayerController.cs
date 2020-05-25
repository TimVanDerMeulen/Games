using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private GameObject camera;

    public float speed = 0.7F;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        camera = transform.Find("Camera").gameObject;
    }

    void Update()
    {
        /*
        float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.AddForce(movement * transform.rotation. * speed);
        */
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;

        //        transform.localPosition += transform.right * h;
        //        transform.localPosition += transform.forward * v;

        Vector3 RIGHT = camera.transform.TransformDirection(Vector3.right);
        Vector3 FORWARD = camera.transform.TransformDirection(Vector3.forward);

        RIGHT.y = 0;
        FORWARD.y = 0;

        transform.localPosition += RIGHT * h;
        transform.localPosition += FORWARD * v;
    }
}
