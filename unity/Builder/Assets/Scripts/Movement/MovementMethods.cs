using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface MovementMethods {
	void SetTarget(GameObject target);
	
	Quaternion GetRotation();
	void ApplyMovement();
}