using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCorntroller : MonoBehaviour {
    
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    
    #region Controls
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    #endregion

    #region Variables

    [Header("Camera")] 
    [SerializeField] private CameraController _cameraController;

    [Header("Custom Variables")]
    [SerializeField] private float walkSSpeed = 1f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float rotationSpeed = 500f;

    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController;
    private Quaternion targetRotation;
    private bool Move = true;
    private bool canSprint = true;
    bool press = false;
    #endregion

    #region Builder

    private void Awake()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        if (Move)
        {
            Movement();
        } 
    }

    #endregion

    #region Custom Methods

    private void Movement() {
        float move_h = Input.GetAxis("Horizontal");
        float move_v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(move_h) + Mathf.Abs(move_v));

        var moveInput = (new Vector3(move_h, 0, move_v)).normalized;
        var moveDir = _cameraController.PlanearRotation * moveInput;

        if (moveAmount > 0) {
            
            if (!IsSprinting) {
                moveAmount = Mathf.Clamp(moveAmount, 0f, 0.1f);
                press = true;
            }else {
                if (press) {
                    moveAmount = 0.0f;
                    press = false;
                }
            }

            _characterController.Move(moveDir * (IsSprinting ? sprintSpeed : walkSSpeed) * Time.deltaTime);
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
            rotationSpeed * Time.deltaTime);

        
        _animator.SetFloat("moveAmount", moveAmount,0f,Time.deltaTime);

    }
    

    #endregion
    
    
}
