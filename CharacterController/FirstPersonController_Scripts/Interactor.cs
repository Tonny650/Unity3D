using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Interactor : MonoBehaviour
{
    #region Variables
    
    [Header("Functional Options")] 
    [SerializeField] private bool canInteract = true;
    [SerializeField] private float distance = 1.5f;
    [SerializeField] private Texture2D puntero;
    [SerializeField] private GameObject textDetect;
    [SerializeField] private InventorySystem _inventorySystem;
    private GameObject ultimoReconocido = null;
    private LayerMask mask;
    
    
    #endregion
    
    #region Custom Controls

    [Header("Controls")] 
    [SerializeField] private KeyCode interactiveKey = KeyCode.F;

    #endregion

    #region BuiltIn Methods
    
    void Start()
    {
        mask = LayerMask.GetMask("Raycast Detect");
        _inventorySystem = GetComponent<InventorySystem>();
        textDetect.SetActive(false);
    }

    
    void Update()
    {
        if (canInteract) {
            handle_Interact();
        }
    }
    
    #endregion

    #region Custom Methods
    //Raycast(source, direction, out hit, distance, mask)
    private void handle_Interact() {
        if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out RaycastHit hit, distance, mask)){
            
            Deselect();
            SelectObject(hit.transform);
            /*
            if (hit.collider.tag == "Interactive/TOOL") {
                //hit.collider.transform.GetComponent<script>().function();
                if (Input.GetKeyDown(interactiveKey)) {
                    hit.collider.transform.GetComponent<Catch>().PickUpObject();
                }
            }*/
            switch (hit.collider.tag) {
                
                case "Interactive/GUNS/G3":
                    if (PressKey()) {
                        hit.collider.transform.GetComponent<Guns>().PickUpObject();
                        _inventorySystem.PickUpItem(hit.collider.GetComponent<ItemObject>());
                    }
                    break;
                case "Interactive/CAJA":
                    if (PressKey()) {
                        _inventorySystem.PickUpItem(hit.collider.GetComponent<ItemObject>());
                    }
                    break;
                case "Interactive/TOOL":
                    if (PressKey()) hit.collider.transform.GetComponent<Catch>().PickUpObject();
                        break;
                default:
                    break;
                
            }
            
        }
        else
        {
            Deselect();
        }
    }

    private void SelectObject(Transform transform) {
        ultimoReconocido = transform.gameObject;
    }

    private void Deselect()
    {
        if (ultimoReconocido)
        {
            ultimoReconocido = null;
        }
    }

    private void OnGUI() {
        Rect rect = new Rect(Screen.width / 2, Screen.height / 2, puntero.width, puntero.height);
        GUI.DrawTexture(rect,puntero);

        if (ultimoReconocido)
        {
            textDetect.SetActive(true);
        }
        else
        {
            textDetect.SetActive(false);
        }
    }

    private bool PressKey() {
        if (Input.GetKeyDown(interactiveKey)) {
            return true;
        }
        else {
            return false;
        }
        
    }
    

    #endregion
}
