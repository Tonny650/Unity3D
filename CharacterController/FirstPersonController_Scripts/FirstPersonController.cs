using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;


public class FirstPersonController : MonoBehaviour
{

    #region Properties

    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && _characterController.isGrounded;

    private bool ShouldCrouch =>
        Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && _characterController.isGrounded;

    #endregion

    #region Custom Controls

    [Header("Controls")] 
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.C;

    #endregion

    #region Variables

    [Header("Funcrional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool useFootsteos = true;
    [SerializeField] private bool willSlideOnSlopes = true;
    [SerializeField] private bool canFall = true;
    


    [Header("Movement Parameters")] 
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float slopeSpeed = 5.5f;

    [Header("Movement Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 10)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 10)] private float lowerLookLimit = 80.0f;

    [Header("Jumping parameters")] 
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch parameters")] 
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrounch = 0.25f;
    [SerializeField] private Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Headbob Parameters")] 
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;
    
    //Sliding parameters
    private Vector3 hitPointNormal;

    private bool IsSliding {
        get
        {
            if (_characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopleHit, 2f)) {

                hitPointNormal = slopleHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > _characterController.slopeLimit;

            }else {
                return false;
            }
        }
    }
    

    [Header("Footstep Parameters")] 
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultipler = 1.5f;
    [SerializeField] private float sprintStepMultipler = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] defaultClips = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] metalClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    [SerializeField] private AudioClip[] waterClips = default;
    private float footstepTimer = 0;
    
    [Header("Fall Parameters")]
    [SerializeField] private AudioSource fallAudioSource = default;
    [SerializeField] private AudioClip[] jumpClips = default;
    [SerializeField] private AudioClip[] fallClips = default;
    private bool isJump;

    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultipler :
        IsSprinting ? baseStepSpeed * sprintStepMultipler : baseStepSpeed;


    private Camera playerCamera;
    private CharacterController _characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    #endregion

    #region BuiltIn Methods

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput(); 
            HandleMouseLook();
            
            if (canJump) { HandleJump(); }

            if (canCrouch) { HandleCrouch(); }

            if (canUseHeadbob) { HandleHeadbob(); }

            if (useFootsteos) { Handle_Footsteps(); }

            if (canFall) { Handle_Fall(); }
            
            ApplyFinalMomevents();
        }
        
    }

    #endregion

    #region Custom Methods

    private void HandleMovementInput()
    {
        currentInput =
            new Vector2((IsSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed) * Input.GetAxis("Vertical"),
                (IsSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection
                                                                                                (Vector3.right) *
                                                                                            currentInput.y);

        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedY, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump)
        {
            footstepAudioSource.PlayOneShot(jumpClips[Random.Range(0, jumpClips.Length - 1)]);
            moveDirection.y = jumpForce;
        }
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void HandleHeadbob()
    {
        if (!_characterController.isGrounded)
        {
            return;
        }

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) *
                (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }

    }

    private void Handle_Footsteps()
    {
        if (!_characterController.isGrounded)
        {
            return;
        }

        if (currentInput == Vector2.zero)
        {
            return;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                
                switch (hit.collider.tag)
                {
                    case "Footsteps/WOOD":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footsteps/METAL":
                        footstepAudioSource.PlayOneShot(metalClips[Random.Range(0, metalClips.Length - 1)]);
                        break;
                    case "Footsteps/GRASS":
                        footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                        break;
                    case "Footsteps/WATER":
                        footstepAudioSource.PlayOneShot(waterClips[Random.Range(0, waterClips.Length - 1)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(defaultClips[Random.Range(0, defaultClips.Length - 1)]);
                        break;
                }

            }
            

            footstepTimer = GetCurrentOffset;
        }

    }
    
    private void ApplyFinalMomevents()
    {
        if (!_characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (willSlideOnSlopes && IsSliding) {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }

        _characterController.Move(moveDirection * Time.deltaTime);

    }

    #endregion

    #region Functions

    private IEnumerator CrouchStand()
    {

        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = _characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchCenter;
        Vector3 currentCenter = _characterController.center;

        while (timeElapsed < timeToCrounch)
        {
            _characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrounch);
            _characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrounch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _characterController.height = targetHeight;
        _characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;


    }

    private void Handle_Fall() {

        if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            if (isJump && _characterController.isGrounded) {
                fallAudioSource.PlayOneShot(fallClips[Random.Range(0, fallClips.Length - 1)]);
                isJump = false;
            }
        }
        if (!hit.collider && !_characterController.isGrounded) {
            isJump = true;
        }
        
    } 
    
    #endregion

    #region Function Public

    public void Inventory() {
        CanMove = !CanMove;
        if (!CanMove) {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    #endregion
}
