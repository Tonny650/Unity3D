using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController _characterController;
    
    [Header("Movimiento")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    [Header("Camera")] 
    public Camera cam;
    public float mouseHorizontal = 3.0f;
    public float mouseVertical = 2.0f;
    public float minRotation = -65.0f;
    public float maxRoration = 60.0f;
    float h_mouse, v_mouse;

    private Vector3 move = Vector3.zero;
    [Header("Animator")] 
    public ControllerAnimator _animator;

    private bool APUNTAR,DISPARAR,Permiso;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        APUNTAR = false;
        DISPARAR = false;
        Permiso = false;
    }

    
    void Update()
    {
        movimiento();
    }

    private void movimiento()
    {
        h_mouse = mouseHorizontal * Input.GetAxis("Mouse X");
        v_mouse += mouseVertical * Input.GetAxis("Mouse Y");

        v_mouse = Mathf.Clamp(v_mouse, minRotation, maxRoration);
        cam.transform.localEulerAngles = new Vector3(-v_mouse,0,0);
        transform.Rotate(0,h_mouse,0);
        
        if (_characterController.isGrounded) {
            move = new Vector3(Input.GetAxis("Horizontal"),0.0f, Input.GetAxis("Vertical"));
            
            
            if (move != Vector3.zero)
            {
                _animator.Activar(true);
            }
            else
            {
                _animator.Activar(false);
            }
            
            
            if (Input.GetKey(KeyCode.LeftShift)) {
                move = transform.TransformDirection(move) * runSpeed;
            } else {
                move = transform.TransformDirection(move) * walkSpeed;
            }

            

            if (Input.GetKey(KeyCode.Space)) {
                move.y = jumpSpeed;
            }
            
        }
        apuntar();
        move.y -= gravity * Time.deltaTime;

        _characterController.Move(move * Time.deltaTime);
    }

    private void apuntar() {
        
        if (Input.GetMouseButtonDown(1)) {
            APUNTAR = !APUNTAR;
            _animator.Apuntar(APUNTAR);
            Permiso = !Permiso;
        }

        if (Permiso)
        {
            if (Input.GetMouseButtonDown(0)) {
                _animator.Disparar();
            }
        }
        
    }
    
    
}
