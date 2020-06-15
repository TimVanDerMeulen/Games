using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElevatorController : PlatformController
{
    public float minHeight, maxHeight;
    public bool moveDown;

    public float movementSpeed;

    public bool moving = false;
    // private int playersOnPlatform = 0;

    private GroundController groundController;

    new void Start(){
        if(minHeight > maxHeight)
            throw new InvalidCastException("min needs to be lower than or equal to max!");
        ResetToValidArea();
        base.Start();

        if(movementSpeed < 0)
            movementSpeed = 0;

        groundController = GetComponentInParent<GroundController>();
    }
    new void Update() {
        base.Update();
        if(moving)
            MoveToLimit(moveDown);
    }

	// public void OnCollisionEnter(Collision collision){
	// 	if(collision.gameObject != null && playersOnPlatform == 0){
	// 		PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
	// 		if(playerController != null){
    //             if(playersOnPlatform == 0)
	// 			    UpdateMovementDirection(!moveDown);
    //             playersOnPlatform++;
	// 		}
	// 	}
	// }
	
	// public void OnCollisionExit(Collision collision){
	// 	if(collision.gameObject != null && collision.gameObject.GetComponent<PlayerController>() != null)
    //         playersOnPlatform--;
	// }
    public void UpdateMovementDirection(bool down){
        moveDown = down;
        moving = true;
    }

    private void MoveToLimit(bool lowerLimit){
        Vector3 currentPos = transform.position;
        float diff = currentPos.y - (lowerLimit ? minHeight : maxHeight);

        float currentSpeed = movementSpeed * Time.deltaTime;

        bool end = Math.Abs(diff) < Math.Max(currentSpeed, 0.1);

        if(end)
            currentPos.y -= diff;
        else
            if(movementSpeed == 0)
                currentPos.y -= diff * Time.deltaTime;
            else
                currentPos.y -= (diff < 0 ? (-1) : 1) * currentSpeed;

        transform.position = currentPos;

        if(end)
            moving = false;

       // ResetToValidArea();
       UpdateNavMesh();
    }

    private void ResetToValidArea(){
        if(transform.position.y > maxHeight)
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        if(transform.position.y < minHeight)
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
    }

    private void UpdateNavMesh(){
        groundController.UpdateNavMesh();
    }

}
