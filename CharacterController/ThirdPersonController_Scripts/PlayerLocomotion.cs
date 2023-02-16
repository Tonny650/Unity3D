using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisName {
    public class PlayerLocomotion : MonoBehaviour {
  
        
        #region Custom Variables

        private Transform cameraObject;
        private InputHandler _inputHandler;
        private Vector3 moveDirection;

        [HideInInspector] 
        public Transform myTransform;

        [HideInInspector] public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Stats")] 
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float rotationSeed = 10f;


        #endregion

        //--------------------------------------------------------

        #region Builder

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();
        }

        public void Update() {
            float delta = Time.deltaTime;
            
            _inputHandler.TickInput(delta);
            HandleMovement(delta);
            HandleRollingAndSprinting(delta);
            
            
        }

        #endregion

        //--------------------------------------------------------
        
        

        //--------------------------------------------------------

        #region Custom Methods

        #region Movement

        private Vector3 normalVector;
        private Vector3 targetPosition;

        private void HandleRotation(float delta) {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = _inputHandler.moveAmount;

            targetDir = cameraObject.forward * _inputHandler.vertical;
            targetDir += cameraObject.right * _inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero) 
                targetDir = myTransform.forward;

            float rs = rotationSeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;

        }

        public void HandleMovement(float delta) {
            
            moveDirection = cameraObject.forward * _inputHandler.vertical;
            moveDirection += cameraObject.right * _inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0; 

            float speed = movementSpeed;
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection , normalVector);
            rigidbody.velocity = projectedVelocity;
            
            animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount,0);

            if (animatorHandler.canRotate) {
                HandleRotation(delta);
            }
            
        }


        #endregion

        public void HandleRollingAndSprinting(float delta) {
            if (animatorHandler.anim.GetBool("isInteracting")) 
                return;

            if (_inputHandler.rollFlag) {
                moveDirection = cameraObject.forward * _inputHandler.vertical;
                moveDirection += cameraObject.right * _inputHandler.horizontal;

                if (_inputHandler.moveAmount > 0) {
                    animatorHandler.PlayTargetAnimation("RollForward",true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                } else
                {
                    animatorHandler.PlayTargetAnimation("RollBackward",true);    
                }
            }
        }
        
        
        #endregion

        //--------------------------------------------------------

        #region Methods Public



        #endregion

    }
    
}
