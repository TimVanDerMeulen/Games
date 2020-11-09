using UnityEngine;

public class BuilderCameraController : MonoBehaviour
{
	[Header("Controls Settings")]
	public KeyCode BUTTON_MOVE_FORWARD = KeyCode.W;
	public KeyCode BUTTON_MOVE_BACKWARD = KeyCode.S;
	public KeyCode BUTTON_MOVE_LEFT = KeyCode.A;
	public KeyCode BUTTON_MOVE_RIGHT = KeyCode.D;
	public KeyCode BUTTON_MOVE_UP = KeyCode.Space;
	public KeyCode BUTTON_MOVE_DOWN = KeyCode.LeftShift;
	
	[Header("Movement Settings")]
	public float movementSpeedHorizontal = 3f;
	public float movementSpeedVertical = 3f;
	
	//[Header("Movement Settings")]
   //[Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
   //public float boost = 3.5f;

    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
    public float positionLerpTime = 0.7f;

    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;
	
	
    class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        public void SetFromTransform(Transform t)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
            x = t.position.x;
            y = t.position.y;
            z = t.position.z;
        }

        public void Translate(Vector3 translation, bool horizontal)
        {
			if (horizontal) {
				Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;			
			
				x += rotatedTranslation.x;
				// ignore y because that is handled in vertical
				z += rotatedTranslation.z;
			}else{
				y += translation.y;
			}
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);
            
            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw, roll);
            t.position = new Vector3(x, y, z);
        }
    }
    
    CameraState m_TargetCameraState = new CameraState();
    CameraState m_InterpolatingCameraState = new CameraState();

    void OnEnable()
    {
        m_TargetCameraState.SetFromTransform(transform);
        m_InterpolatingCameraState.SetFromTransform(transform);
    }

    Vector3 GetInputTranslationDirectionHorizontal()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(BUTTON_MOVE_FORWARD))
        {
            direction += Vector3.forward * movementSpeedHorizontal;
        }
        if (Input.GetKey(BUTTON_MOVE_BACKWARD))
        {
            direction += Vector3.back * movementSpeedHorizontal;
        }
        if (Input.GetKey(BUTTON_MOVE_LEFT))
        {
            direction += Vector3.left * movementSpeedHorizontal;
        }
        if (Input.GetKey(BUTTON_MOVE_RIGHT))
        {
            direction += Vector3.right * movementSpeedHorizontal;
        }
        return direction;
    }
	
	Vector3 GetInputTranslationDirectionVertical()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(BUTTON_MOVE_DOWN))
        {
            direction += Vector3.down * movementSpeedVertical;
        }
        if (Input.GetKey(BUTTON_MOVE_UP))
        {
            direction += Vector3.up * movementSpeedVertical;
        }
        return direction;
    }
    
    void Update()
    {
        // Exit Sample  

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; 
			#endif
        }
        // Hide and lock cursor when right mouse button pressed
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Unlock and show cursor when right mouse button released
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Rotation
        if (Input.GetMouseButton(1))
        {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));
            
            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
            m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
        }
        
        // Translation
        var translationHorizontal = GetInputTranslationDirectionHorizontal() * Time.deltaTime;
        var translationVertical = GetInputTranslationDirectionVertical() * Time.deltaTime;
        
        // Modify movement by a boost factor (defined in Inspector and modified in play mode through the mouse scroll wheel)
        //boost += Input.mouseScrollDelta.y * 0.2f;
        //translation *= Mathf.Pow(2.0f, boost);

        m_TargetCameraState.Translate(translationHorizontal, true);
        m_TargetCameraState.Translate(translationVertical, false);

        // Framerate-independent interpolation
        // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
        var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
        var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
        m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

        m_InterpolatingCameraState.UpdateTransform(transform);
    }
}