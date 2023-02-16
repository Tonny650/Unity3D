using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisName {
    public class CameraHandler : MonoBehaviour {

        #region Custom Variables
        public Transform TargetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ingoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float follwSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;

        public float cameraShereRadius = 0.2f;
        public float cameraCollisionOffSet = 0.2f;
        public float minimumCollisionOffSet = 0.2f;
        
        [Header("Pivot Range")]
        [SerializeField] [Tooltip("Limite Minimo de la rotacion de la camara en el eje -Y")]public float minimumPivot = -35;
        [SerializeField] [Tooltip("Limite Maximo de la rotacion de la camara en el eje Y")] public float maximumPivot = 35;
        
        #endregion

        #region Builder
        
        private void Awake()
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ingoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        
        #endregion

        #region Custom Methods
        
        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, TargetTransform.position,
                ref cameraFollowVelocity, delta / follwSpeed);
            myTransform.position = targetPosition;
            
            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput) {

            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCameraCollision(float delta) {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraShereRadius,direction, out hit,
                Mathf.Abs(targetPosition), ingoreLayers)) {

                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffSet);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffSet) {
                targetPosition = -minimumCollisionOffSet;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;

        }
        

        #endregion

    }
}
