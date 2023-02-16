using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Custom Variables

    [SerializeField] Transform followTarget;

    [Header("Rotation Speed")] 
    [SerializeField] float rotationSpeedX = 2f;
    [SerializeField] float rotationSpeedY = 2f;
    
    [Header("Custom variables")]
    [SerializeField] float distance = 5;
    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;

    [SerializeField] Vector2 framingOffset;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    
    float rotationX;
    float rotationY;
    
    private float invertXval;
    private float invertYval;
    
    #endregion
    
    #region Builder
    void Start() {
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update() {

        invertXval = (invertX) ? -1 : 1;
        invertYval = (invertY) ? -1 : 1;
        
        rotationX += Input.GetAxis("Mouse Y") * invertYval * rotationSpeedY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        rotationY += Input.GetAxis("Mouse X") * invertXval * rotationSpeedX;
        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focusPosition = followTarget.position + new Vector3(framingOffset.x,framingOffset.y);
        
        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }
    
    
    #endregion

    #region Public Methods

    public Quaternion PlanearRotation => Quaternion.Euler(0, rotationY, 0);

    public Quaternion GetPlanerRotation() {
        return Quaternion.Euler(0,rotationY,0);
    }

    #endregion
}
