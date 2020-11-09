using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour, CollisionBroadcaster.Subscriber
{
	[Header("Camera Target")]
	public GameObject target;
	
	[Header("Camera Settings")]
	public bool snapCamera = true;
	public float distance = 10;
	public float RotationSpeed = 5;
    
	private Vector3 currentOffset;
	private Vector3 defaultOffsetGround, defaultOffsetAir;
	private CollisionBroadcaster currentCollisionBroadcaster;
	private bool airborn = true;
	
	protected bool rotateCam = false;
 
	/*
	########################## Game Loop ##########################
	*/
 
    public void Start() {
		this.ReAdjust();
    }
		
	public void Update(){
		if(this.target == null){
			Debug.Log("target null");
			return;
		}
		
		this.toggleMode();
		this.UpdateCameraPosition();
	}
	
	public void targetCollisionEnter(GameObject target, Collision collision){
		//DetectGroundContact(true);
	}
	
	public void targetCollisionExit(GameObject target, Collision collision){
		//DetectGroundContact(false);
	}
	
	public void targetTriggerEnter(GameObject target, Collider other){
		DetectGroundContact(true);
	}
	
	public void targetTriggerExit(GameObject target, Collider other){
		DetectGroundContact(false);
	}
	
	/*
	########################## Other ##########################
	*/
	
	public virtual void ReAdjust(){
		// collisionDetection
		var targetCollisionBroadcaster = target.GetComponent<CollisionBroadcaster>();
		
		// ######### stop if same target
		if(targetCollisionBroadcaster != null && targetCollisionBroadcaster == currentCollisionBroadcaster)
			return; // should be same target (no setup needed)
		
		// ######### detach old target
		if(currentCollisionBroadcaster != null)
			currentCollisionBroadcaster.unsubscribe(this);
		
		// ######### setup new target
		currentCollisionBroadcaster = target.GetComponent<CollisionBroadcaster>();
		currentCollisionBroadcaster.subscribe(this);
		
		// setup base rotation of camera
		Quaternion targetRotation = this.target.transform.rotation;
        defaultOffsetGround = targetRotation * (new Vector3(0, -1, 2).normalized * distance);
        defaultOffsetAir = targetRotation * (new Vector3(0, -1, 5).normalized * distance);
	}
	
	/*
	########################## Protected ##########################
	*/
	
	protected bool IsTouchingGround(){
		return !this.airborn;
	}
	
	protected bool IsFlying(){
		return this.airborn;
	}
	
	protected virtual void DetectGroundContact(bool groundContact){		
		this.airborn = !groundContact;
		
		UpdateCameraPosition();
	}
	
	/*
	########################## Private ##########################
	*/
	
	private void UpdateCameraPosition(){
		if (rotateCam) {
			Quaternion turnHorizontal = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
			Quaternion turnVertical = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotationSpeed * (-1), Vector3.right);
			currentOffset = turnHorizontal * (turnVertical * currentOffset);
		}else {
			if(snapCamera)
				currentOffset = (IsFlying() ? defaultOffsetAir : defaultOffsetGround);
		}
		
		Quaternion targetRotation = this.target.transform.rotation;
		this.transform.position = this.target.transform.position - (snapCamera ? (targetRotation * currentOffset) : currentOffset);
		
		if(IsFlying())
			this.transform.rotation = this.target.transform.rotation;
		else
			this.transform.LookAt(this.target.transform);
	}
	
	
	private void toggleMode(){
		if (Input.GetMouseButtonDown(1))
        {
            //Cursor.lockState = CursorLockMode.Locked;
			rotateCam = true;
        }
	
        if (Input.GetMouseButtonUp(1))
        {
			//Cursor.lockState = CursorLockMode.None;
			rotateCam = false;
        }
	}
	
}